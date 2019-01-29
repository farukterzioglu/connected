using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Configuration.Repositories.ADO.Model;
using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Repositories.ADO
{
    public class DynamicRepositoryContext
    {
        public bool LogRequired { get; set; }
    }

    public class DynamicRepository<T> : IDisposable where T : EntityBase, new()
    {
        DynamicRepositoryContext _rc = null;
        public DynamicRepository()
        {
            if (_rc == null)
                _rc = new DynamicRepositoryContext() { LogRequired = true };
        }

        public DynamicRepository(DynamicRepositoryContext rc)
        {
            _rc = rc;
        }


        public T Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public void Update(T obj, T original = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public T GetById(T obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
