using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using NLog;
using WebUI.Models;

namespace WebUI.Controllers
{
    [HandleError(View = "Error")]
    public class TrainController : Controller
    {
        private ITrainService _trainService;
        private ICarriageService _carriageService;
        private IMapper _mapper;
        private Logger _logger;

        public TrainController(ITrainService trainService, IMapper mapper, ICarriageService carriageService, Logger logger)
        {
            _trainService = trainService;
            _mapper = mapper;
            _carriageService = carriageService;
            _logger = logger;
        }

        public async Task<ActionResult> ManageTrains()
        {
            return View(await GetAllTrains());
        }

        [Authorize(Roles = "moder")]
        public async Task<ActionResult> DeleteTrain(int trainId)
        {
            _logger.Info(nameof(DeleteTrain) + " " + nameof(trainId) + " " + trainId);

            await _trainService.DeleteTrain(trainId);

            var trains = await GetAllTrains();

            return PartialView("TrainsList", trains);
        }

        private async Task<List<Train>> GetAllTrains()
        {
            var trains = await _trainService.GetAll();

            return trains.Where(t => !t.IsDeleted).ToList();
        }

        [Authorize(Roles = "moder")]
        public async Task<ActionResult> EditTrain(int trainId)
        {
            _logger.Info(nameof(EditTrain) + " " + nameof(trainId) + " " + trainId);

            var train = await _trainService.GetById(trainId);
            train.Carriages = train.Carriages.Where(c => !c.IsDeleted).ToList();
            var model = _mapper.Map<TrainViewModel>(train);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        [ActionName("EditTrain")]
        public async Task<ActionResult> UpdateTrain(TrainViewModel model)
        {
            _logger.Info(nameof(DeleteTrain) + " " + nameof(model.Id) + " " + model.Id);

            var train = _mapper.Map<Train>(model);

            await _trainService.UpdateTrain(train);

            return RedirectToAction("ManageTrains");
        }

        [Authorize(Roles = "moder")]
        public async Task<ActionResult> DeleteCarriage(int carriageId, int editedTrainId)
        {
            _logger.Info(nameof(DeleteCarriage) + " " + nameof(carriageId) + " " + carriageId);

            var carriage = await _carriageService.GetById(carriageId);

            await _carriageService.DeleteCarriage(carriageId, editedTrainId);

            return RedirectToAction("EditTrain", new { trainId = editedTrainId });
        }

        [Authorize(Roles = "moder")]
        public ActionResult CarriageCreationForm(int trainId)
        {
            var carriageVm = new CarriageViewModel();
            carriageVm.TrainId = trainId;

            return PartialView("CarriageCreate", carriageVm);
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        public ActionResult CreateCarriage(CarriageViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.Info(nameof(CreateCarriage) + " " + nameof(model.Id) + " " + model.Id);

                var carriage = _mapper.Map<Carriage>(model);
                var train = _carriageService.AddCarriageToTrain(carriage, model.TrainId);
                ViewBag.ErrorMessage = "Carriage data incorrect, please, try again.";
            }

            return RedirectToAction("EditTrain", new { trainId = model.TrainId });
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public ActionResult CreateTrain()
        {
            _logger.Info(nameof(CreateTrain));

            var train = _trainService.CreateTrain();
            var trainViewModel = _mapper.Map<TrainViewModel>(train);

            return View("EditTrain", trainViewModel);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }
    }
}