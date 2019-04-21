using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View("Error");
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            return View("Error");
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }

        public ActionResult NotAuthorized()
        {
            Response.StatusCode = 403;
            return View("Error");
        }
    }
}