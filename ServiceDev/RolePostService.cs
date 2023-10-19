using Dao.IDao;
using Model.Entitys;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.ServiceDev
{
    public class RolePostService :BaseService<RolePostRelation>
    {
        private readonly IRolePostDao irolePostDao;

        public RolePostService(IRolePostDao irolePostDao)
        {
            this.irolePostDao = irolePostDao;
            base._ibaseDao = irolePostDao;
        }
    }
}
