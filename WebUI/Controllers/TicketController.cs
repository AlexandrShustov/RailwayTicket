using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebUI.Identity;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TicketController : Controller
    {
        private IRouteService _routeService;
        private ICarriageService _carriageService;
        private ITrainService _trainService;
        private UserManager<IdentityUser, Guid> _userManager;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private IMapper _mapper;
        private IUserService _userService;
        private ITicketService _ticketService;

        public TicketController(IRouteService routeService, ICarriageService carriageService, IMapper mapper, ITrainService trainService, UserManager<IdentityUser, Guid> userManager, IUserService userService, ITicketService ticketService)
        {
            _routeService = routeService;
            _carriageService = carriageService;
            _mapper = mapper;
            _trainService = trainService;
            _userManager = userManager;
            _userService = userService;
            _ticketService = ticketService;
        }

        public async Task<ActionResult> BuyTicket(int routeId)
        {
            var route = await _routeService.GetById(routeId);

            var vm = new TicketViewModel(route.Stations, route.Train.Carriages);
            vm.TrainNumber = route.Train.Number;
            vm.RouteId = route.Id;
            vm.TrainId = route.Train.Id;

            return View(vm);
        }

        public async Task<ActionResult> CarriageScheme(int carriageId)
        {
            var carriage = await _carriageService.GetById(carriageId);

            return View("CarriagePlacesSchema", carriage);
        }

        [ActionName("BuyTicket")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> MakeTicket(TicketViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var ticket = _mapper.Map<Ticket>(vm);

                await _trainService.TakePlace(vm.TrainId, vm.CarriageNumber, vm.PlaceNumber);
                var userName = AuthenticationManager.User.Identity.Name;
                var user = await _userService.FindByEmailAsync(userName);
                ticket.PassangerName =  user.FirstName + " " + user.LastName;

                _ticketService.GenerateTicket(ticket);
            }

            return RedirectToAction("DownloadPage");
        }

        [Authorize]
        public FileResult TicketFileDownload(string filePath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = "ticket.pdf";

            return response;
        }

        [Authorize]
        public ActionResult DownloadPage()
        {
            return View();
        }
    }
}