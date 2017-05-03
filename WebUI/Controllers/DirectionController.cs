using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class DirectionController : Controller
    {
        [HttpGet]
        public ActionResult DirectionList()
        {
            return View();
        }
    }
}