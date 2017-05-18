using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using ILogger = Microsoft.Owin.Logging.ILogger;

namespace WebUI.Controllers
{
    public class AboutController : Controller
    {
        private Logger _logger;

        public AboutController(Logger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult About()
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