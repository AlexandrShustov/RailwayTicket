using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Microsoft.AspNet.Identity;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;

        public AdminController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> AllUsers()
        {
            var users =  await _userService.GetAll();   

            return View(users.Where(u => !_userService.IsInRole(u.UserId, "admin")).ToList());
        }

        [HttpGet]
        public ActionResult UserDetails(Guid userId)
        {
            var user = _userService.FindById(userId);
            var vm = new UserViewModel();
            vm.User = user;

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserViewModel model)
        {
            await _userService.ChangeRoleTo(model.SelectedRole, model.User.UserId);

            return RedirectToAction("AllUsers");
        }
    }
}