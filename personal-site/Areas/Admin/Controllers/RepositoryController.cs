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

        public async Task<ActionResult> EditRepository(AdminRepoEditViewModel model)
        {
            string jsonStr = await RepositoryService.GetInstance().LoadRepositories();
            Debug.WriteLine(jsonStr);
            return ControllerHelper.JsonActionResponse(true, "Successfully saved repository");
        }

        public ActionResult RemoveRepository(string repoName)
        {
            return ControllerHelper.JsonActionResponse(true, "Successfully removed repository");
        }
    }
}