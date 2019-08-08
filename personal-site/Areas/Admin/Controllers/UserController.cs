using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;
using personal_site.Services;

namespace personal_site.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View("../User");
        }

        public async Task<ActionResult> RemoveUser(string userId)
        {
            UserService userService = UserService.GetInstance();
            bool removeStatus = await userService.RemoveUser(userId);

            if (removeStatus)
                return ControllerHelper.JsonActionResponse(true, "Successfully removed user");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove user");
        }
    }
}