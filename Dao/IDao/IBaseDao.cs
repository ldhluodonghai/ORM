using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.IDao
{
    public interface IBaseDao<TEntity> where TEntity : class, new()
    {
        TEntity Get();
        TEntity FindByName(string name);
        Task<TEntity> FindByNameAsync(string name);
        [Obsolete]
        TEntity FindById(Guid id);
        TEntity FindByAnyId(Guid id);
        TEntity FindByPhoneNumber(string phoneNumber);
        int DeleteById(Guid id);
        List<TEntity> GetAll();
        List<TEntity> GetAllById(Guid id);
        int Add(TEntity entity);
        int Update(TEntity entity);

        
        //TEntity Delete(string name);





    }
}
