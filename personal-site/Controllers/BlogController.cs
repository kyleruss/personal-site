using Newtonsoft.Json;
using personal_site.Models;
using personal_site.Services;
using System;
using System.Collections.Generic;
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
            List<BlogPost> blogList = await blogService.GetBlogList();
            var blogListJson = JsonConvert.SerializeObject(blogList);

            return Json(blogListJson, JsonRequestBehavior.AllowGet);
        }
    }
}