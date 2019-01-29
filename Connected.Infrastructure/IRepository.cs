using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T>  FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T>   FindAll(params Expression<Func<T, object>>[] includeProperties);

        T Add(T entity);
        bool Update(T entity);
        void Remove(T entity);
    }
}
