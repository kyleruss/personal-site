using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using personal_site.Controllers;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Areas.Admin.Controllers
{
    public class LoginController : AbstractAuthController
    {
        //================================
        // LOGIN ACTIONS
        //================================
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View("../Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AdminLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("../Login", viewModel);

            AccountService accService = AccountService.GetInstance();
            bool verifyUserResult = await accService.VerifySystemUser(viewModel.Username, viewModel.Password, UserManager);

            if (!verifyUserResult)
                return ControllerHelper.JsonActionResponse(false, "Invalid login");
            else
            {
                bool authCodeSent = await accService.SendAuthCode();
                if (!authCodeSent) return ControllerHelper.JsonActionResponse(false, "Failed to send authentication code");
                else return ControllerHelper.JsonActionResponse(true, "An authentication code has been sent");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
    }
}