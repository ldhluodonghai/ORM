using Dao.DaoIpml;
using Dao.IDao;
using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : BaseService<User>
    {
        private readonly IUserDao _ibaseDao;
        private readonly IRoleDao _roleDao;
        private readonly IUserRoleDao _userRoleDao;
        private readonly IPostDao _postDao;
        private readonly IRolePostDao _rolePostDao;

        public UserService(IUserDao ibaseDao, IRoleDao roleDao, IUserRoleDao userRoleDao, IPostDao postDao, IRolePostDao rolePostDao)
        {
            _roleDao = roleDao;
            base._ibaseDao = ibaseDao;
            _userRoleDao = userRoleDao;
            _postDao = postDao;
            _rolePostDao = rolePostDao;
        }

       
        //查看用户用那些资源
        public IList<Post> GetPosts(Guid userId)
        {
            List<Post> posts = new List<Post>();
            UserRoleRelation userRoleRelation = _userRoleDao.FindByAnyId(userId);
            Role role = _roleDao.FindByAnyId(userRoleRelation.RoleId);     
            List<RolePostRelation> rolePostRelations = _rolePostDao.GetAllById(role.Id);
            foreach (RolePostRelation relation in rolePostRelations)
            {
                Post post = _postDao.FindByAnyId(relation.PostId);
                posts.Add(post);
            }
            return posts;
           
        }
    }
}
