using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class UserGroupRelation
    {
        public Guid Id { get; private set; }
        public Guid UserId { get;  set; }
        public Guid GroupId { get; set; }

        public UserGroupRelation()
        { 

        }

        public UserGroupRelation( Guid userId, Guid groupId)
        {
            Id =Guid.NewGuid();
            UserId = userId;
            GroupId = groupId;
        }
    }
}
