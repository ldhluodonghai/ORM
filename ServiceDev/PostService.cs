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
    public class PostService :BaseService<Post>
    {
        private readonly IPostDao ipostDao;

        public PostService(IPostDao ipostDao)
        {
            this.ipostDao = ipostDao;
            base._ibaseDao = ipostDao;
        }
    }
}
