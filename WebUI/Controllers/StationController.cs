using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class StationController : Controller
    {
        private IStationService _stationService;
        private IMapper _mapper;

        public StationController(IStationService stationService, IMapper mapper)
        {
            _stationService = stationService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult CreateStation()
        {
            return View();
        }

        public ActionResult CreateNewStation(Station station)
        { 
            var stationViewModel = new StationViewModel();
            return PartialView("CreateNewStation", stationViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        [ActionName("CreateStation")]
        public async Task<ActionResult> AddStation(StationViewModel station)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = "Field Name is required.";
                return RedirectToAction("ManageStations");
            }
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
    }
}