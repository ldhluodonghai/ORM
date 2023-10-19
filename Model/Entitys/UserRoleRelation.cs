using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public  class UserRoleRelation
    {
        
        public Guid Id { get;private set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public UserRoleRelation()
        {
            Id = Guid.NewGuid();
        }

        public UserRoleRelation( Guid userId, Guid guid)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            RoleId = guid;
        }
    }
}
