using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class StationController : Controller
    {
        private IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet]
        public ActionResult CreateStation()
        {
            return View();
        }

        public ActionResult CreateNewStation(Station station)
        { 
            return PartialView("CreateNewStation");
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        [ActionName("CreateStation")]
        public ActionResult AddStation(Station station)
        {
            _stationService.CreateStation(station);

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