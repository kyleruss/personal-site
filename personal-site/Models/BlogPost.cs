using Newtonsoft.Json;
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

        public string Description { get; set; }

        public string PostContent { get; set; }

        public string PostImage { get; set; }

        [JsonIgnore]
        public virtual List<BlogPostComment> Comments { get; set; }

        public DateTime TimePosted { get; set; }
    }
}