using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class RepositoryController : Controller
    {
        public ActionResult Index()
        {
            return View("../Repository");
        }

        public ActionResult EditRepository(AdminRepoEditViewModel model)
        {
            return ControllerHelper.JsonActionResponse(true, "Successfully saved repository");
        }

        public ActionResult RemoveRepository(int index)
        {
            return ControllerHelper.JsonActionResponse(true, "Successfully removed repository");
        }
    }
}