using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using personal_site.Filters;

namespace personal_site.Areas.Portfolio.Controllers
{
    [RouteArea("Portfolio")]
    [SiteModeFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Policy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }
    }
}