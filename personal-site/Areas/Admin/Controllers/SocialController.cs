using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SocialController : Controller
    {
        public ActionResult Index()
        {
            return View("../SocialMedia");
        }

        public ActionResult SaveSocialMediaInfo(AdminSocialMediaViewModel model)
        {
            SocialMediaService socialService = SocialMediaService.GetInstance();
            bool updateStatus = socialService.UpdateSocialConfig(model);

            if (updateStatus)
                return ControllerHelper.JsonActionResponse(true, "Successfully updated social settings");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to update social settings");
        }
    }
}