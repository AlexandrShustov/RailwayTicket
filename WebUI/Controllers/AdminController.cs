using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Microsoft.AspNet.Identity;
using NLog;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private Logger _logger;

        public AdminController(IUserService userService, IMapper mapper, Logger logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> AllUsers()
        {
            _logger.Info(nameof(AllUsers));

            var users =  await _userService.GetAll();   

            return View(users.Where(u => !_userService.IsInRole(u.UserId, "admin")).ToList());
        }

        [HttpGet]
        public ActionResult UserDetails(Guid userId)
        {
            _logger.Info(nameof(UserDetails) + " " + nameof(userId) + "=" + userId.ToString());

            var user = _userService.FindById(userId);
            var vm = new UserViewModel();
            vm.User = user;

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserViewModel model)
        {
            _logger.Info(nameof(UpdateUser) + " " + nameof(model.User.UserId) + "=" + model.User.UserId.ToString());

            await _userService.ChangeRoleTo(model.SelectedRole, model.User.UserId);

            return RedirectToAction("AllUsers");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }
    }
}