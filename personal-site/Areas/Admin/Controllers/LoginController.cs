using Microsoft.AspNet.Identity.Owin;
using personal_site.Controllers;
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

            var loginResult = await SignInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

            if (loginResult == SignInStatus.Success)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            else
            {
                ModelState.AddModelError("", "Failed to login");
                return View("../Login", viewModel);
            }
        }
    }
}