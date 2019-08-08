using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using personal_site.Controllers;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class UserController : AbstractAuthController
    {
        public ActionResult Index()
        {
            return View("../User");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveUser(string userId)
        {
            UserService userService = UserService.GetInstance();
            bool removeStatus = await userService.RemoveUser(userId);

            if (removeStatus)
                return ControllerHelper.JsonActionResponse(true, "Successfully removed user");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove user");
        }

        [HttpPost]
        public async Task<ActionResult> SaveUser(UserEditViewModel model)
        {
            UserService userService = UserService.GetInstance();
            bool saveSuccess;

            if (model.UserId == null)
                saveSuccess = await userService.CreateUser(model, UserManager);
            else
                saveSuccess = await userService.EditUser(model, UserManager);

            if (saveSuccess)
                return ControllerHelper.JsonActionResponse(true, "Successfully saved user");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to save user");
        }
    }
}