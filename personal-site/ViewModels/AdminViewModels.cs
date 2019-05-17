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

    public class AdminBlogEditModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public int PostId { get; set; }
    }

    public class AdminEntityRemovalModel
    {
        [Required]
        public int EntityId { get; set; }
    }
}