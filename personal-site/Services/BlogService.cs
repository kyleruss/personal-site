﻿using personal_site.Models;
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
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

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

        public async Task<string> GetBlogComments(int PostId)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                List<BlogPostComment> commentList = await context.BlogPostComments.
                    Where(x => x.PostId == PostId).ToListAsync();

                return JsonConvert.SerializeObject(commentList);
            }
        }

        public async Task<BlogPostComment> CreateComment(CommentViewModel commentModel, string userId)
        {
            BlogPostComment comment;
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                comment = new BlogPostComment()
                {
                    CommentContent = commentModel.Content,
                    PostId = commentModel.PostId,
                    CommenterId = userId
                };

                context.BlogPostComments.Add(comment);
                await context.SaveChangesAsync();

                return comment;
            };
        }

        public async Task<bool> UserExists(string email)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
                return await context.Users.AnyAsync(x => x.UserName == email);
        }


        public static BlogService GetInstance()
        {
            _instance = _instance ?? new BlogService();
            return _instance;
        }
    }
}