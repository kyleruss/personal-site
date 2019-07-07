using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Areas.Admin.Controllers
{
    public class RssController : Controller
    {
        public ActionResult Index()
        {
            return View("../Rss");
        }

        [HttpPost]
        public JsonResult UpdateRssChannel(AdminRssViewModels model)
        {
            RssService service = RssService.GetInstance();
            bool serviceReq = service.UpdateChannel(model.ChannelUpdateModel);

            if (serviceReq)
                return ControllerHelper.JsonActionResponse(true, "Saved Channel Settings");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to save Channel Settings");
        }


        [HttpPost]
        public ActionResult PushRssUpdate(AdminRssViewModels model)
        {
            RssService service = RssService.GetInstance();
            bool serviceReq = service.PushUpdate(model.ItemPushModel);

            if(serviceReq)
                return ControllerHelper.JsonActionResponse(true, "Pushed Update");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to push update");
        }

        [HttpPost]
        public JsonResult RemoveRssItem(int id)
        {
            RssService service = RssService.GetInstance();
            bool serviceReq = service.RemoveItem(id);

            if(serviceReq)
                return ControllerHelper.JsonActionResponse(true, "Removed RSS Item");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove RSS Item");
        }
    }
}