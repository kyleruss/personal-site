using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.Services.AuthHandlers;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public async Task<ActionResult> Index()
        {
            int followCount = await new TwitterAuthHandler().GetTwitterFollowerCount();
            ViewBag.FollowerCount = followCount;

            return View("../Dashboard");
        }

        [HttpPost]
        public ActionResult ShutdownMode(bool enableMode)
        {

            Debug.WriteLine("enable: " + enableMode);
            DashboardService dashboardService = DashboardService.GetInstance();
            bool toggleStatus = dashboardService.ToggleShutdownMode(enableMode);

            if(toggleStatus)
                return ControllerHelper.JsonActionResponse(true, "Shutdown mode has been successfully toggled");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to toggle shutdown mode");
        }

        [HttpPost]
        public ActionResult MaintenanceMode(bool enableMode)
        {
            DashboardService dashboardService = DashboardService.GetInstance();
            bool toggleStatus = dashboardService.ToggleMaintenanceMode(enableMode);

            if (toggleStatus)
                return ControllerHelper.JsonActionResponse(true, "Maintenance mode has been successfully toggled");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to toggle maintenance mode");
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