﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using personal_site.Models;

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

    public class AdminRssViewModels
    {
        public AdminRssChannelViewModel ChannelUpdateModel { get; set; }

        public AdminRssItemViewModel ItemPushModel { get; set; }
    }

    public class AdminRssItemViewModel
    {
        [Required]
        public string ItemContent { get; set; }

        public string ItemLink { get; set; }

        public string ItemTitle { get; set; }
    }

    public class AdminSocialMediaViewModel
    {
        [Required]
        public string Twitter { get; set; }

        [Required]
        public string Github { get; set; }

        [Required]
        public string Dribble { get; set; }

        [Required]
        public string Rss { get; set; }

        [Required]
        public string StackOverflow { get; set; }
    }

    public class AdminRepoTaskViewModel
    {
        [Required]
        public int TaskId { get; set; }
    }

    public class AdminRepoEditViewModel
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Languages { get; set; }

        public string Readme { get; set; }

        public int CodeLines { get; set; }
    }

    public class AdminUserEditViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string ProfilePicture { get; set; }

        public string RoleName { get; set; } 
    }

    public class AdminUserStatViewModel
    {
        public List<AdminUserMonthlyStatsModel> MonthlyStatsModel { get; set; }

        public AdminUserCountStatsModel UserCountStats { get; set; }
    }

    public class AdminUserCountStatsModel
    {
        public int MonthlyCount { get; set; }

        public int TotalCount { get; set; }
    }

    public class AdminUserMonthlyStatsModel
    {
        public int Month { get; set; }

        public int UserCount { get; set; }
    }

    public class AdminUserInfoModel
    {
        public ApplicationUser User { get; set; }

        public string RoleName { get; set; }
    }
}