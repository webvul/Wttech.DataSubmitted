using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
                return View();
        }

        public ActionResult Error()
        {
                return View("Error");
        }

    }
}