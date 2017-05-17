using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [HandleError(View = "Error")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }
    }
}