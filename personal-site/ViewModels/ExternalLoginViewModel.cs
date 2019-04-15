using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personal_site.ViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}