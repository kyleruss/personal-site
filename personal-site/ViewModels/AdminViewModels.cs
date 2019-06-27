using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personal_site.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AdminBlogEditViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public string PostImage { get; set; }

        public int? PostId { get; set; }
    }

    public class AdminEntityRemovalViewModel
    {
        [Required]
        public int EntityId { get; set; }
    }

    public class AdminRssChannelViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }

    public class AdminRssItemViewModel
    {
        [Required]
        public string ItemContent { get; set; }
    }

    public class AdminSocialMediaViewModel
    {
        [Required]
        public string TwitterLink { get; set; }

        [Required]
        public string GithubLink { get; set; }

        [Required]
        public string DribbleLink { get; set; }

        [Required]
        public string RssLink { get; set; }

        [Required]
        public string StackOverflowLink { get; set; }
    }

    public class AdminRepoTaskViewModel
    {
        [Required]
        public int TaskId { get; set; }
    }
}