using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace WebUI.Controllers
{
    [HandleError(View = "Error")]
    public class HomeController : Controller
    {
        private Logger _logger;

        public HomeController(Logger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Error(filterContext.Exception, filterContext.Exception.Message);

            filterContext.Result = View("Error");
        }
    }
}