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
        private IMailSender _mailSender;

        private static readonly string _ticketFilePath = AppDomain.CurrentDomain.BaseDirectory + "/Ticket.pdf";

        public TicketController(IRouteService routeService, ICarriageService carriageService, IMapper mapper, ITrainService trainService, UserManager<IdentityUser, Guid> userManager, IUserService userService, ITicketService ticketService, IMailSender mailSender)
        {
            _routeService = routeService;
            _carriageService = carriageService;
            _mapper = mapper;
            _trainService = trainService;
            _userManager = userManager;
            _userService = userService;
            _ticketService = ticketService;
            _mailSender = mailSender;
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
                var route = await _routeService.GetById(vm.RouteId);
                var userName = AuthenticationManager.User.Identity.Name;
                var user = await _userService.FindByEmailAsync(userName);

                var departureTime = route.Stations.First(s => s.Station.Name == ticket.DepartureStationName).DepartureTime;

                if (departureTime != null)
                {
                    ticket.DepartureTime = departureTime.Value;
                }

                var arriveTime = route.Stations.First(s => s.Station.Name == ticket.ArriveStationName).ArriveTime;

                if (arriveTime != null)
                {
                    ticket.ArriveTime = arriveTime.Value;
                }


                await _trainService.TakePlace(vm.TrainId, vm.CarriageNumber, vm.PlaceNumber);

                ticket.PassangerName = user.FirstName + " " + user.LastName;
                ticket.RelatedUserId = user.UserId;

                ticket.TeaCount = vm.TeaCount;
                ticket.IsNeedLinen = vm.IsNeedLinen;

                var price = await _ticketService.CountTicketPrice(ticket.RouteId, 
                                                                  ticket.DepartureStationName, 
                                                                  ticket.ArriveStationName, 
                                                                  ticket.TeaCount, 
                                                                  ticket.IsNeedLinen);
                ticket.Price = price;

                await _ticketService.CreateTicket(ticket);
                _ticketService.GeneratePdfTicket(ticket);
            }

            return RedirectToAction("DownloadPage");
        }

        [Authorize]
        public FileResult TicketFileDownload()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(_ticketFilePath);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = "ticket.pdf";

            return response;
        }

        [Authorize]
        public ActionResult DownloadPage()
        {
            return View();
        }

        [Authorize]
        public ActionResult TicketFileSendMail(string userMail)
        {
            _mailSender.SendEmail(userMail);

            return View("SendSuccess");
        }

        [Authorize]
        public ActionResult SendTicketForm()
        {
            var userMail = AuthenticationManager.User.Identity.Name;
            ViewBag.userMail = userMail;

            return View();
        }
    }
}