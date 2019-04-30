using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Controllers
{
    public class AdminController : AbstractAuthController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLoginViewModel viewModel)
        {
            return View();
        }

        [Authorize]
        public ActionResult ControlPanel()
        {
            return View();
        }
    }
}