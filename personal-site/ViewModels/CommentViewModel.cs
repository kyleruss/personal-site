using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personal_site.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public string Content { get; set; }

        public string User { get; set; }
    }
}