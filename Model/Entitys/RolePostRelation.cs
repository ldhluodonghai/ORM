using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class RolePostRelation
    {
        public Guid Id { get;private set; }
        public Guid RoleId { get; set; }
        public Guid PostId { get; set; }


        public RolePostRelation( Guid roleId, Guid postId)
        {
            Id=Guid.NewGuid();
            RoleId = roleId;
            PostId = postId;
        }

        public RolePostRelation()
        {
           
        } public RolePostRelation(Guid id)
        {
            Id = id;
        }
    }
}
