using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.IDao;
using UserManagement.Db;

namespace Dao.DaoIpml
{
    public class BaseDao<TEntity> :ORMSqlHelper<TEntity>, IBaseDao<TEntity> where TEntity : class, new()
    {
        
        public TEntity Delete(string name)
        {
            throw new NotImplementedException("通过删除");
        }

        public TEntity Find(string name)
        {
            return FindByName(name);
        }
        public TEntity Find(Guid id)
        {
            return FindById(id);
        }
        public TEntity FindByAnyId(Guid id)
        {
            return FindByAnyIds(id);
        }

        public TEntity FindByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public int DeleteById(Guid id) 
        {
            return Delete(id);
        }
        public TEntity Get()
        {
           
            throw new NotImplementedException();
        }

        public int Create(TEntity entity)
        {
            /*int row = sqlHelper.ExecteSql<TEntity>("",entity);
            return row;*/
            return Add(entity);
        }

        public int Update(TEntity entity)
        {
           return Edit(entity);
        }

        public List<TEntity> GetAll()
        {
            return GetList();
        }

        public Task<TEntity> FindByNameAsync(string name)
        {
            return FindAsync(name);

        }

        public List<TEntity> GetAllById(Guid roleId)
        {
            return FindRolePostsById(roleId);
        }
    }
}
