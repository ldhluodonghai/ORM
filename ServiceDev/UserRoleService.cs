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
    public class UserRoleService : BaseService<UserRoleRelation>
    {
        private readonly IUserRoleDao iuserRoleDao;

        public UserRoleService(IUserRoleDao iuserRoleDao)
        {
            this.iuserRoleDao = iuserRoleDao;
            base._ibaseDao = iuserRoleDao;
        }
    }
}
