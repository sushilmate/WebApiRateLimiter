namespace WebApiRateLimiter.Data.Interface
{
    using System.Collections.Generic;

    public interface IRepository<T> where T : class
    {
        void Create(T entity);

        void Delete(IEnumerable<int> ids);

        IEnumerable<T> GetAll();

        T Get(int id);

        bool Update(T entity);

        bool UpdateAll(IEnumerable<T> entities);
    }
}