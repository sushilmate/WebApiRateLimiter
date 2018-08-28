using System;
using System.Collections.Generic;
using WebApiRateLimiter.Data.Interface;

namespace WebApiRateLimiter.Data.Repository
{
    /// <summary>
    /// Empty Repository base class
    /// when we connect to db then we will write the actual methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
        {
        }

        protected void Save()
        {
        }

        public void Create(T entity)
        {
        }

        public void Delete(T entity)
        {
        }

        public bool Update(T entity)
        {
            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return null;
        }

        public T Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateAll(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}