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
    public class GroupService :BaseService<Group>
    {
        private readonly IGroupDao igroupDao;

        public GroupService(IGroupDao igroupDao)
        {
            this.igroupDao = igroupDao;
            base._ibaseDao = igroupDao;
        }
    }
}
