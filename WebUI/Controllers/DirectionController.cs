using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Abstract;
using Domain.Entities;
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

            return View(await GetAllRoutes());
        }

        [HttpGet]
        public async Task<ActionResult> ManageRoutes()
        {
            return View(await GetAllRoutes());
        }

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
                vm.FreePlacesCount = route.Train.Carriage.Sum(x => x.Places.Count(p => p.IsFree));

                routeViewModels.Add(vm);
            }

            return routeViewModels;
        }
    }
}