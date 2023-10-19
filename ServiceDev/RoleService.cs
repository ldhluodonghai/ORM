using Dao.DaoIpml;
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
    public class RoleService : BaseService<Role>
    {
        public readonly IRoleDao _iroleDao;
        public RoleService(IRoleDao ibaseDao) 
        {
            _iroleDao= ibaseDao;
            base._ibaseDao= ibaseDao;
        }
    }
}
