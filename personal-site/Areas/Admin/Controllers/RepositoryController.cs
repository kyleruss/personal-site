using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;
using personal_site.Services;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class RepositoryController : Controller
    {
        public ActionResult Index()
        {
            return View("../Repository");
        }

        [ValidateInput(false)]
        public async Task<ActionResult> EditRepository(AdminRepoEditViewModel model)
        {
            await RepositoryService.GetInstance().EditRepository(model, Server);
            return ControllerHelper.JsonActionResponse(true, "Successfully saved repository");
        }

        public async Task<ActionResult> RemoveRepository(string repoName)
        {
            await RepositoryService.GetInstance().RemoveRepository(repoName, Server);
            return ControllerHelper.JsonActionResponse(true, "Successfully removed repository");
        }
    }
}