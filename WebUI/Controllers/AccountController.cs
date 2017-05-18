using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;
using NLog.Fluent;
using WebUI.Identity;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly RoleManager<IdentityRole, Guid> _roleManager;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private IMapper _mapper;
        private IUserService _userService;
        private Logger _logger;

        public AccountController(UserManager<IdentityUser, Guid> userManager, IUserService userService, IMapper mapper, RoleManager<IdentityRole, Guid> roleManager, Logger logger)
        {
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
            _roleManager = roleManager;
            _logger = logger;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            _logger.Info(nameof(SignInAsync) + " " + user.UserName);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindAsync(model.Email, model.Password);

                if (user != null)
                {
                    _logger.Info(nameof(Login) + " " + user.UserName + " success");

                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    _logger.Info(nameof(Login) + " error");

                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

           
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email };
                var result = _userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.Info(nameof(Register) + " " + user.UserName + " success");

                    var entityUser = _mapper.Map<User>(model);
                    entityUser.PasswordHash = user.PasswordHash;
                    entityUser.SecurityStamp = user.SecurityStamp;
                    entityUser.UserId = user.Id;
                    entityUser.UserName = user.UserName;
                    await _userService.UpdateAsync(entityUser);

                    await SignInAsync(user, isPersistent: false);

                    return RedirectToAction("HomePage", "Home");
                }

                AddErrors(result);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _logger.Info(nameof(LogOff) + " " + AuthenticationManager.User.Identity.Name);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("HomePage", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("HomePage", "Home");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }

    }
}