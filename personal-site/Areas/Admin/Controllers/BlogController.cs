using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using personal_site.Helpers;
using personal_site.Models;
using personal_site.Services;
using personal_site.ViewModels;

namespace personal_site.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        public async Task<ActionResult> Index()
        {
            List<BlogPost> blogList = await BlogService.GetInstance().GetBlogPosts();
            ViewBag.BlogList = blogList;

            return View("../Blog");
        }

        public async Task<ActionResult> GetBlogList()
        {
            BlogService blogService = BlogService.GetInstance();
            string blogListJson = await blogService.GetBlogsPostsJson();
            return ControllerHelper.JsonObjectResponse(blogListJson);
        }

        public async Task<ActionResult> SaveBlogPost(AdminBlogEditViewModel model)
        {
            BlogService blogService = BlogService.GetInstance();
            BlogPost savedBlogPost = await blogService.SaveBlogPost(model);

            if (savedBlogPost != null)
                return ControllerHelper.JsonActionResponse(true, "Saved Blog Post");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to save blog post");
        }

        public async Task<ActionResult> RemoveBlogPost(AdminEntityRemovalViewModel model)
        {
            BlogService blogService = BlogService.GetInstance();
            bool blogRemoved = await blogService.RemoveBlogPost(model.EntityId);

            if (blogRemoved)
                return ControllerHelper.JsonActionResponse(true, "Blog post has been removed");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove blog post");
        }

        public async Task<ActionResult> RemoveBlogComment(AdminEntityRemovalViewModel model)
        {
            BlogService blogService = BlogService.GetInstance();
            bool commentRemoved = await blogService.RemoveBlogPostComment(model.EntityId);

            if (commentRemoved)
                return ControllerHelper.JsonActionResponse(true, "Comment has been removed");
            else
                return ControllerHelper.JsonActionResponse(false, "Failed to remove comment");
        }

    }
}