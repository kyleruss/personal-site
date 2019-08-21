using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using personal_site.Controllers;
using personal_site.Helpers;
using personal_site.Models;
using personal_site.Services;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class UserController : AbstractAuthController
    {
        public ActionResult Index()
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles)
                roleList.Add(new SelectListItem() { Value = role.Name, Text = role.Name });

            ViewBag.RoleList = roleList;
            
            return View("../User");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveUser(string id)
        {
            UserService userService = UserService.GetInstance();
            bool removeStatus = await userService.RemoveUser(id);

            if (removeStatus)
                return ControllerHelper.JsonActionResponse(true, "Successfully removed user");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove user");
        }

        public async Task<ActionResult> GetUserInfo(string id)
        {
            UserService userService = UserService.GetInstance();
            ApplicationUser user = await userService.GetUser(id, UserManager);
            string userRole = await userService.GetUserRole(id, UserManager);
            AdminUserInfoModel infoModel = new AdminUserInfoModel() { User = user, RoleName = userRole };

            string userJson = JsonConvert.SerializeObject(infoModel);
            return ControllerHelper.JsonObjectResponse(userJson);
        }

        [HttpPost]
        public async Task<ActionResult> SaveUser(AdminUserEditViewModel model)
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