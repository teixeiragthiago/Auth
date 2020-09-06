using System.Collections.Generic;

namespace Auth.Domain.Common.Repository
{
    public interface IRepository<T> where T : class
    {
        int Post(T entity);
        void Delete(T item);        
        void Put(T item);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(out int total, int? page = null, int? paginateQuantity = null);
        T GetById(int id);
    }
}