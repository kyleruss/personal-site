using personal_site.Controllers;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Login(AdminLoginViewModel viewModel)
        {
            return View("../Login");
        }
    }
}