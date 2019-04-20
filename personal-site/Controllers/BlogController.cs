using Newtonsoft.Json;
using personal_site.Helpers;
using personal_site.Models;
using personal_site.Services;
using personal_site.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace personal_site.Controllers
{
    public class BlogController : Controller
    {
        public async Task<JsonResult> GetBlogPosts()
        {
            BlogService blogService = BlogService.GetInstance();
            string blogsJson = await blogService.GetBlogs();
            return Json(blogsJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> PostComment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ControllerHelper.GetModelStateErrors(ModelState);
                return ControllerHelper.JsonActionResponse(false, "Please check your comment", errorList);
            }

            else
            {
                BlogService blogService = BlogService.GetInstance();
                BlogPostComment savedComment = await blogService.CreateComment(model);

                if (savedComment != null)
                    return ControllerHelper.JsonActionResponse(true, "Saved comment", null, savedComment);
                else
                    return ControllerHelper.JsonActionResponse(false, "Failed to save comment");
            }
        }

        public ActionResult SocialAuthCallback(string message)
        {
            var model = new AuthCallbackViewModel() { ResponseMessage = message };
            return View(model);
        }

        public async Task<JsonResult> GetBlogComments(int PostId)
        {
            BlogService blogService = BlogService.GetInstance();
            string commentsJson = await blogService.GetBlogComments(PostId);
            return Json(commentsJson, JsonRequestBehavior.AllowGet);
        }
    }
}