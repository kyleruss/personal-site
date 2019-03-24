using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace personal_site.Services
{
    public class BlogService
    {
        private static BlogService _instance;

        private BlogService() { }

        public void GetBlogList()
        {

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