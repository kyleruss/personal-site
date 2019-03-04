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

        public virtual List<BlogPostComment> Comments { get; set; }

        [DefaultValue("getdate()")]
        public DateTime TimePosted { get; set; }
    }
}