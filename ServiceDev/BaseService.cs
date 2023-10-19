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
    public class BaseService<T> where T : class,new()
    {
        protected  IBaseDao<T> _ibaseDao;


        public T Find(string name)
        {
            return _ibaseDao.FindByName(name);
        }
        public Task<T> FindAsync(string name)
        {
            return _ibaseDao.FindByNameAsync(name);
        }
        public T Find(Guid id)
        {
            return _ibaseDao.FindById(id);
        }
        public T FindAny(Guid id)
        {
            return _ibaseDao.FindByAnyId(id);
        }
        public int Create(T entity)
        {
            return _ibaseDao.Add(entity);
        }
        public List<T> GetAll()
        {
            return _ibaseDao.GetAll();
        }
        public int Delete(Guid id) 
        {
            return _ibaseDao.DeleteById(id);
        }
        public int EditByEntity(T entity)
        {
            return _ibaseDao.Update(entity);
        }

    }
}
