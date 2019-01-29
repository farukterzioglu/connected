using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using Connected.Infrastructure;

namespace Connected.DAL.Repositories.EntityFramework
{
    public abstract class AbstractRepository<T>  : IRepository<T> where T : class
    {
        public virtual IEnumerable<T>   FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = DataContextFactory.GetDataContext().Set<T>();

            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                    items = items.Include(includeProperty);
            return items.Where(predicate);
        }
        public virtual IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = DataContextFactory.GetDataContext().Set<T>();

            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                    items = items.Include(includeProperty);
            return items;
        }

        public virtual T Add(T entity)
        {
            return DataContextFactory.GetDataContext().Set<T>().Add(entity);
        }
        //TODO : test this -> 
        public virtual bool Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            DataContextFactory.GetDataContext().Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return true;
        }
        public virtual void Remove(T entity)
        {
            DataContextFactory.GetDataContext().Set<T>().Remove(entity);
        }
        public void Dispose()
        {
            if (DataContextFactory.GetDataContext() != null)
            {
                DataContextFactory.GetDataContext().Dispose();
            }
        }
    }
}
