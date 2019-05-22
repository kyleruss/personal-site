using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Areas.Admin.Controllers
{
    public class SocialController : Controller
    {
        public ActionResult Index()
        {
            return View("../SocialMedia");
        }
    }
}