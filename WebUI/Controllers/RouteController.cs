using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using Domain.Enumerations;
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
        [AllowAnonymous]
        public async Task<ActionResult> RouteList()
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

            List<RouteViewModel> routeViewModels = _mapper.Map<List<RouteViewModel>>(routesList);

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
        public async Task<ActionResult> EditRoute(int routeId, string errors = "")
        {
            var route = await _routeService.GetById(routeId);

            var viewModel = _mapper.Map<RouteEditViewModel>(route);
            var trains = await _trainService.GetAll();

            ViewBag.Errors = errors;
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

        [Authorize(Roles = "moder")]
        public ActionResult CreateStation()
        {
            return View();
        }

        [Authorize(Roles = "moder")]
        public async Task<ActionResult> AddRouteStation(RouteStationCreateViewModel vm)
        {
            vm.AllStations = await _stationService.GetAll();
            var routeStation = _mapper.Map<RouteStation>(vm);

            if (routeStation.DepartureTime <= routeStation.ArriveTime)
            {
                return RedirectToAction("EditRoute", new {routeId = vm.RouteId, errors = "Departure time must be greater than arrive time."});
            }

            await _routeStationService.AddStationToRoute(vm.RouteId, routeStation);

            return RedirectToAction("EditRoute", new { routeId = vm.RouteId });
        }

        [Authorize(Roles = "moder")]
        public ActionResult RemoveRouteStation(int routeId, int stationId)
        {
            _routeService.RemoveStationFromRoute(routeId, stationId);

            return RedirectToAction("EditRoute", new { routeId = routeId});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RouteDetails(int routeId)
        {
            var route = await _routeService.GetById(routeId);

            var detailRouteVm = _mapper.Map<DetailsRouteViewModel>(route);

            return View(detailRouteVm);
        }

        public ActionResult AutocompleteRouteSearch(string term)
        {
            var models = _stationService.FindByTerm(term).Select(s => s.Name);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> FindRoutes(SearchViewModel model)
        {
            var results = await _routeService.GetRoutesBetweenStations(model.StationFrom, model.StationTo);
            var vm = _mapper.Map<List<RouteViewModel>>(results);

            return View("SearchResults", vm);
        }
    }
}