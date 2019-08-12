using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;

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
            DashboardService dashboardService = DashboardService.GetInstance();
            dashboardService.ToggleMaintenanceMode((mode > 0 ? true : false));
            return ControllerHelper.JsonActionResponse(true, "Shutdown mode has been enabled");
        }

        public ActionResult MaintenanceMode(int mode)
        {
            DashboardService dashboardService = DashboardService.GetInstance();
            dashboardService.ToggleMaintenanceMode((mode > 0 ? true : false));
            return ControllerHelper.JsonActionResponse(true, "Maintenance mode has been enabled");
        }

        [HttpGet]
        public ActionResult GetUserStatisticData()
        {
            AdminUserStatViewModel userRegistrationStats = DashboardService.GetInstance().GetUserRegistrationData();
            string monthlyDataJson = JsonConvert.SerializeObject(userRegistrationStats);
            return ControllerHelper.JsonObjectResponse(monthlyDataJson);
        }
    }
}