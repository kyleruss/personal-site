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

        [DefaultValue("getdate()")]
        public DateTime PostedDate { get; set; }

        [ForeignKey("BlogPost")]
        public int PostId { get; set; }
        public virtual BlogPost BlogPost { get; set; }
    }
}