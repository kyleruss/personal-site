using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace personal_site.Models
{
    public class UserAccessTokens
    {
        [Key]
        public int AccessId { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}