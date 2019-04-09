using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace personal_site.Models
{
    public class BlogPost
    {
        [Key]
        public int PostId { get; set; }

        public string Title { get; set; }

        public string PostContent { get; set; }

        public string PostImage { get; set; }

        public virtual List<BlogPostComment> Comments { get; set; }


        private DateTime? postedDate = null;

        public DateTime TimePosted
        {
            get { return this.postedDate ?? DateTime.Now; }
            set { this.postedDate = value; }
        }
    }
}