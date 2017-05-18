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
    public class FeedbackController : Controller
    {
        private IFeedbackService _feedbackService;
        private IMapper _mapper;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private IUserService _userService;

        public FeedbackController(IFeedbackService feedbackService, IMapper mapper, IUserService userService)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var feedbacks = _feedbackService.GetAll();

            var feedbacksModels = _mapper.Map<List<FeedbackViewModel>>(feedbacks);

            feedbacksModels.ForEach( f =>
            {
                var user = _userService.FindById(f.RelatedUserId);
                f.UserName = user.FirstName + " " + user.LastName;
            });

            var feedbackListViewModel = new FeedbackListViewModel();
            feedbackListViewModel.Feedbacks = feedbacksModels;
            feedbackListViewModel.NewFeedback = new FeedbackViewModel();

            ViewBag.IsAuthorized = AuthenticationManager.User.Identity.IsAuthenticated;

            return View("FeedbackList", feedbackListViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> LeaveFeedback(FeedbackListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = "Please, input a correct data.";
                return View();
            }

            var feedback = _mapper.Map<Feedback>(model.NewFeedback);
            feedback.RelatedUserId = _userService.FindByEmail(AuthenticationManager.User.Identity.Name).UserId;

            await _feedbackService.CreateFeedback(feedback);

            return RedirectToAction("About", "About");
        }
    }
}