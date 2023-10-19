using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys.Dto
{
    public class PostDto
    {
       
        public string? Name { get; set; }

        public int PostType { get; set; }

        public string Url { get; set; }
        public string ParentId { get; set; }

        public PostDto(string? name, int postType, string url, string parentId)
        {
            Name = name;
            PostType = postType;
            Url = url;
            ParentId = parentId;
        }

    }
}
