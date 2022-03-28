using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PostDto
    {
        public string Title { get; set; }
        public DateTime PubDate { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
