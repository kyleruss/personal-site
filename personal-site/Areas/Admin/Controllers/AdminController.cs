using personal_site.Controllers;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Areas.Admin.Controllers
{
    public class AdminController : AbstractAuthController
    {
        //================================
        // LOGIN ACTIONS
        //================================
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


        //================================
        // DASHBOARD ACTIONS
        //================================
        [AllowAnonymous]
        public ActionResult Dashboard()
        {
            return View();
        }

 

        //================================
        // BLOG ACTIONS
        //================================
        [AllowAnonymous]
        public ActionResult Blog()
        {
            return View();
        }

        //================================
        // RSS ACTIONS
        //================================
        [AllowAnonymous]
        public ActionResult Rss()
        {
            return View();
        }

        //================================
        // SOCIAL ACTIONS
        //================================
        [AllowAnonymous]
        public ActionResult SocialMedia()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Statistics()
        {
            return View();
        }

      
    }
}