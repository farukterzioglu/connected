using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.DAL.Repositories.ADONET
{
    public class Repository<T> where T :class
    {
        public T Insert()
        {
            throw new NotImplementedException();
        }

        public List<T> Get()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
