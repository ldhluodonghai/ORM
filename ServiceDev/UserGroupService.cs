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
    public class UserGroupService : BaseService<UserGroupRelation>
    {
        private readonly IUserGroupDao iuserGroupDao;

        public UserGroupService(IUserGroupDao iuserGroupDao)
        {
            this.iuserGroupDao = iuserGroupDao;
            base._ibaseDao = iuserGroupDao;
        }
    }
}
