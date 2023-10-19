using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Post
    {
        public Guid Id { get;private set; }
        public string? Name { get; set; }

        public int PostType { get; set; }

        public string Url { get ; set; }
        public string ParentId { get; set; }

        public Post(Guid id, string? name, int postType, string url, string parentId)
        {
            Id = id;
            Name = name;
            PostType = postType;
            Url = url;
            ParentId = parentId;
        }

        public Post()
        {
        }
        public Post(Guid id)
        {
            Id=id;
        }
    }
}
