using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace personal_site.Areas.Admin.Controllers
{
    public class RssController : Controller
    {
        // GET: Admin/Rss
        public ActionResult Index()
        {
            return View();
        }
    }
}