using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Abstract;
using Microsoft.Owin.Security;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class DirectionController : Controller
    {
        private IRouteService _routeService;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public DirectionController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<ActionResult> DirectionList()
        {
            ViewBag.IsModer = AuthenticationManager.User.IsInRole("moder");

            var routes = await _routeService.GetAll();

            var routeViewModels = new List<RouteViewModel>();
            foreach (var route in routes)
            {
                var vm = new RouteViewModel();
                vm.Id = route.Id;
                vm.ArriveTime = route.Stations.Last().ArriveTime.Value;
                vm.DepartureTime = route.Stations.First().DepartureTime.Value;
                vm.FirstStationName = route.Stations.First().Station.Name;
                vm.LastStationName = route.Stations.Last().Station.Name;
                vm.FreePlacesCount = route.Train.Carriage.Sum(x => x.Places.Count(p => p.IsFree));

                routeViewModels.Add(vm);
            }
            return View(routeViewModels);
        }

        [HttpGet]
        public async Task<ActionResult> ManageRoutes()
        {
            var routes = await _routeService.GetAll();

            var routeViewModels = new List<RouteViewModel>();
            foreach (var route in routes)
            {
                var vm = new RouteViewModel();
                vm.Id = route.Id;
                vm.ArriveTime = route.Stations.Last().ArriveTime.Value;
                vm.DepartureTime = route.Stations.First().DepartureTime.Value;
                vm.FirstStationName = route.Stations.First().Station.Name;
                vm.LastStationName = route.Stations.Last().Station.Name;
                vm.FreePlacesCount = route.Train.Carriage.Sum(x => x.Places.Count(p => p.IsFree));

                routeViewModels.Add(vm);
            }
            return View(routeViewModels);
        }

        public async Task DeleteRoute(int RouteId)
        {
            await _routeService.DeleteRoute(RouteId);
        }
    }
}