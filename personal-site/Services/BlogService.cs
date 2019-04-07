using personal_site.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace personal_site.Services
{
    public class BlogService
    {
        private static BlogService _instance;

        private BlogService() { }

        public async Task<string> GetBlogs()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                List<BlogPost> blogList = await context.BlogPosts.ToListAsync();
                return JsonConvert.SerializeObject(blogList);
            };
        }

        public void GetBlogPost()
        {

        }

        public void GetBlogPostComments()
        {

        }

        public void CreateComment()
        {

        }

        public static BlogService GetInstance()
        {
            _instance = _instance ?? new BlogService();
            return _instance;
        }
    }
}