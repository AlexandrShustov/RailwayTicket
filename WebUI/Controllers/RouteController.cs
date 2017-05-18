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
using NLog;

namespace WebUI.Controllers
{
    [HandleError(View = "Error")]
    public class RouteController : Controller
    {
        private IRouteService _routeService;
        private IStationService _stationService;
        private ITrainService _trainService;
        private IRouteStationService _routeStationService;
        private IMapper _mapper;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private Logger _logger;

        public RouteController(IRouteService routeService, IStationService stationService, IMapper mapper, ITrainService trainService, IRouteStationService routeStationService, Logger logger)
        {
            _routeService = routeService;
            _stationService = stationService;
            _mapper = mapper;
            _trainService = trainService;
            _routeStationService = routeStationService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RouteList()
        {
            _logger.Info(nameof(this.RouteList) + " " + AuthenticationManager.User.Identity.Name);


            ViewBag.IsModer = AuthenticationManager.User.IsInRole("moder");
            ViewBag.IsAdmin = AuthenticationManager.User.IsInRole("admin");

            return View(await GetAllRoutes());
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> ManageRoutes()
        {
            _logger.Info(nameof(this.ManageRoutes) + " " + AuthenticationManager.User.Identity.Name);

            return View(await GetAllRoutes());
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public async Task<ActionResult> DeleteRoute(int routeId)
        {
            _logger.Info(nameof(this.DeleteRoute) + " " + AuthenticationManager.User.Identity.Name + " routeId: " + routeId);

            await _routeService.DeleteRoute(routeId);

            ViewBag.IsModer = AuthenticationManager.User.IsInRole("moder");

            return View("RouteList", await GetAllRoutes());
        }

        public async Task<List<RouteViewModel>> GetAllRoutes()
        {
            var routes = await _routeService.GetAll();
            var routesList = routes.ToList().Where(r => !r.IsDeleted);

            List<RouteViewModel> routeViewModels = _mapper.Map<List<RouteViewModel>>(routesList);

            _logger.Info(nameof(this.GetAllRoutes) + " " + AuthenticationManager.User.Identity.Name + " result:" + routes.Select(r => r.Id));

            return routeViewModels;
        }

        [HttpGet]
        [Authorize(Roles = "moder")]
        public ActionResult CreateRoute()
        {
            var newRoute = _routeService.CreateRoute();

            _logger.Info(nameof(this.CreateRoute) + " " + AuthenticationManager.User.Identity.Name);

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

            _logger.Info(nameof(this.EditRoute) + " " + AuthenticationManager.User.Identity.Name + " routeId: " + route.Id);

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
            _logger.Info(nameof(this.AddRoute) + " " + AuthenticationManager.User.Identity.Name + " modelId: " + model.Id);

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
        public async Task AddRouteStation(RouteStationCreateViewModel vm)
        {
            var routeStation = _mapper.Map<RouteStation>(vm);
            await _routeStationService.AddStationToRoute(vm.RouteId, routeStation);

            _logger.Info(nameof(this.AddRouteStation) + " " + AuthenticationManager.User.Identity.Name);

            RedirectToAction("EditRoute", new {routeId = vm.RouteId});
        }

        public async Task<ActionResult> IsValidRouteStation(RouteStationCreateViewModel vm)
        {
            vm.AllStations = await _stationService.GetAll();
            var routeStation = _mapper.Map<RouteStation>(vm);

            var creatingRoute = await _routeService.GetById(vm.RouteId);

            if (routeStation.DepartureTime <= routeStation.ArriveTime || routeStation.DepartureTime <= routeStation.ArriveTime)
            {
                return Json("Departure time must be greater than arrive time.");
            }

            if (creatingRoute.Stations.Any(s => s.Station.Name == routeStation.Station.Name))
            {
                return Json("Station with the same name almost exists in route.");
            }

            if (creatingRoute.Stations.Count > 0 &&
                creatingRoute.Stations.Last().DepartureTime >= routeStation.ArriveTime)
            {
                return Json("Arrive time can`t be less or equal than departure time of last added station.");
            }

            await AddRouteStation(vm);

            return Json(new { RedirectUrl = Url.Action("EditRoute", new { routeId = vm.RouteId }) });
        }

        [Authorize(Roles = "moder")]
        public ActionResult RemoveRouteStation(int routeId, int stationId)
        {
            _routeService.RemoveStationFromRoute(routeId, stationId);

            _logger.Info(nameof(this.RemoveRouteStation) + " " + AuthenticationManager.User.Identity.Name + " routeId: " + routeId + " stationId" + stationId);

            return RedirectToAction("EditRoute", new { routeId = routeId});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RouteDetails(int routeId, string errors = null)
        {
            _logger.Info(nameof(this.RouteDetails) + " " + AuthenticationManager.User.Identity.Name + " routeId: " + routeId);

            if (errors != null)
            {
                ViewBag.Errors = errors;
            }

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
            _logger.Info(nameof(this.FindRoutes) + " " + AuthenticationManager.User.Identity.Name + model.StationFrom + " " + model.StationTo);

            ViewBag.IsModer = AuthenticationManager.User.IsInRole("moder");
            ViewBag.IsAdmin = AuthenticationManager.User.IsInRole("admin");
            ViewBag.IsSearchResult = true;

            if (model.StationFrom == model.StationTo)
            {
                ViewBag.Errors = "Stations are the same, please, select a different stations.";
                return RedirectToAction("RouteList");
            }

            var results = await _routeService.GetRoutesBetweenStations(model.StationFrom, model.StationTo);
            var vm = _mapper.Map<List<RouteViewModel>>(results);


            return View("RouteList", vm);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }
    }
}