using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.Owin.Security;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class RouteController : Controller
    {
        private IRouteService _routeService;
        private IStationService _stationService;
        private ITrainService _trainService;
        private IRouteStationService _routeStationService;
        private IMapper _mapper;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public RouteController(IRouteService routeService, IStationService stationService, IMapper mapper, ITrainService trainService, IRouteStationService routeStationService)
        {
            _routeService = routeService;
            _stationService = stationService;
            _mapper = mapper;
            _trainService = trainService;
            _routeStationService = routeStationService;
        }

        [HttpGet]
        public async Task<ActionResult> DirectionList()
        {
            ViewBag.IsModer = AuthenticationManager.User.IsInRole("moder");

            return View(await GetAllRoutes());
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> ManageRoutes()
        {
            return View(await GetAllRoutes());
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> DeleteRoute(int RouteId)
        {
            await _routeService.DeleteRoute(RouteId);

            return View("ManagedRoutesList", await GetAllRoutes());
        }

        public async Task<List<RouteViewModel>> GetAllRoutes()
        {
            var routes = await _routeService.GetAll();
            var routesList = routes.ToList().Where(r => !r.IsDeleted);
            var routeViewModels = new List<RouteViewModel>();

            foreach (var route in routesList)
            {
                var vm = new RouteViewModel();
                var arriveTime = route.Stations.Last().ArriveTime;
                var departureTime = route.Stations.First().DepartureTime;

                vm.Id = route.Id;
                
                if (arriveTime != null)
                {
                    vm.ArriveTime = arriveTime.Value;
                }
                
                if (departureTime != null)
                {
                    vm.DepartureTime = departureTime.Value;
                }

                vm.FirstStationName = route.Stations.First().Station.Name;
                vm.LastStationName = route.Stations.Last().Station.Name;
                vm.FreePlacesCount = route.Train.Carriages.Sum(x => x.Places.Count(p => p.IsFree));

                routeViewModels.Add(vm);
            }

            return routeViewModels;
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public ActionResult CreateRoute()
        {
            var newRoute = _routeService.CreateRoute();

            return RedirectToAction("EditRoute", new { routeId = newRoute.Id });
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> EditRoute(int routeId)
        {
            var route = await _routeService.GetById(routeId);

            var viewModel = _mapper.Map<RouteEditViewModel>(route);
            var trains = await _trainService.GetAll();

            viewModel.Trains = new SelectList(trains.ToList(), "Id", "Number");

            return View("EditRoute", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> RouteStationForm(int routeId)
        {
            return View(await GetRouteCreationViewModel(routeId));
        }

        private async Task<RouteStationCreateViewModel> GetRouteCreationViewModel(int routeId)
        {
            var stations = await _stationService.GetAll();
            var routeStationCreateVm = new RouteStationCreateViewModel();

            routeStationCreateVm.AllStations = stations;
            routeStationCreateVm.StationsSelectItems = new SelectList(routeStationCreateVm.AllStations, "Id", "Name");
            routeStationCreateVm.RouteId = routeId;

            return routeStationCreateVm;
        }

        [HttpPost]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> AddRoute(RouteEditViewModel model)
        {
            if (model.Stations.Count <= 1)
            {
                return RedirectToAction("ManageRoutes");
            }

            var entityRoute = _mapper.Map<Route>(model);

            await _routeService.AddTrainToRoute(model.SelectedTrainId, entityRoute.Id);
            await _routeService.ActivateRoute(entityRoute.Id);
            
            return RedirectToAction("ManageRoutes");
        }

        public ActionResult CreateStation()
        {
            return View();
        }

        public async Task<ActionResult> AddRouteStation(RouteStationCreateViewModel vm)
        {
            vm.AllStations = await _stationService.GetAll();
            var routeStation = _mapper.Map<RouteStation>(vm);

            if (routeStation.DepartureTime >= routeStation.ArriveTime)
            {
                ViewBag.Errors = "Departure time must be less than arrive time.";
                return RedirectToAction("EditRoute", new {routeId = vm.RouteId});
            }

            await _routeStationService.AddStationToRoute(vm.RouteId, routeStation);

            return RedirectToAction("EditRoute", new { routeId = vm.RouteId });
        }

        public ActionResult RemoveRouteStation(int routeId, int stationId)
        {
            _routeService.RemoveStationFromRoute(routeId, stationId);

            return RedirectToAction("EditRoute", new { routeId = routeId});
        }
    }
}