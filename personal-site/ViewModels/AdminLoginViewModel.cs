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
}