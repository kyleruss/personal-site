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
using personal_site.ViewModels;
using System.Diagnostics;

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

        public async Task<int> CreateComment(CommentViewModel commentModel)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                BlogPostComment comment = new BlogPostComment()
                {
                    CommentContent = commentModel.Content,
                    PostId = commentModel.PostId
                };

                context.BlogPostComments.Add(comment);
                return await context.SaveChangesAsync();
            };
        }

        public void GetBlogPostComments()
        {

        }

        public static BlogService GetInstance()
        {
            _instance = _instance ?? new BlogService();
            return _instance;
        }
    }
}