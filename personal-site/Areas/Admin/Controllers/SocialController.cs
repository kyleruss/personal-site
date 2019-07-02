using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class SocialController : Controller
    {
        public ActionResult Index()
        {
            return View("../SocialMedia");
        }

        public ActionResult SaveSocialMediaInfo(AdminSocialMediaViewModel model)
        {

        }
    }
}