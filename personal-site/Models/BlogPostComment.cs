using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace personal_site.Models
{
    public class BlogPostComment
    {
        [Key]
        public int CommentId { get; set; }

        public string CommentContent { get; set; }

        public string CommenterId { get; set; }

        private DateTime? postedDate = null;

        public DateTime PostedDate
        {
            get { return this.postedDate ?? DateTime.Now; }
            set { this.postedDate = value; }
        }

        [ForeignKey("BlogPost")]
        public int PostId { get; set; }

        [JsonIgnore]
        public virtual BlogPost BlogPost { get; set; }
    }
}