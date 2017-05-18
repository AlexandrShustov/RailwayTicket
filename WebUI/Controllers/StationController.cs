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
    public class StationController : Controller
    {
        private IStationService _stationService;
        private IMapper _mapper;
        private Logger _logger;

        public StationController(IStationService stationService, IMapper mapper, Logger logger)
        {
            _stationService = stationService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        public ActionResult CreateStation()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        public ActionResult CreateNewStation(Station station)
        {
            var stationViewModel = new StationViewModel();

            return PartialView("CreateNewStation", stationViewModel);
        }

        public async Task<ActionResult> IsValidStationModel(StationViewModel model)
        {
            var stations = await _stationService.GetAll();

            if (stations.Where(s => !s.IsDeleted).Select(s => s.Name.ToLower()).Contains(model.Name.ToLower()))
            {
                return Json("The station with the same name is already exists.");
            }

            await AddStation(model);

            return Json(new { RedirectUrl = Url.Action("ManageStations")});
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        [ActionName("CreateStation")]
        public async Task<ActionResult> AddStation(StationViewModel station)
        {
            await _stationService.CreateStation(_mapper.Map<Station>(station));

            return RedirectToAction("ManageStations");
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> ManageStations()
        {
            return View(await GetAllStations());
        }

        [Authorize(Roles = "moder")]
        public async Task<ActionResult> DeleteStation(int stationId)
        {
            _logger.Info(nameof(DeleteStation) + " " + nameof(stationId) + " " + stationId);

            await _stationService.DeleteStation(stationId);

            var stations = await GetAllStations();

            return PartialView("StationsList", stations);
        }

        private async Task<List<Station>> GetAllStations()
        {
            var stations = await _stationService.GetAll();
            stations = stations.Where(s => !s.IsDeleted).ToList();

            return stations;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }
    }
}