using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;

namespace personal_site.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View("../Dashboard");
        }

        public ActionResult ShutdownMode(int mode)
        {
            return ControllerHelper.JsonActionResponse(true, "Shutdown mode has been enabled");
        }

        public ActionResult MaintenanceMode(int mode)
        {
            return ControllerHelper.JsonActionResponse(true, "Maintenance mode has been enabled");
        }
    }
}