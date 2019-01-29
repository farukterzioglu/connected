using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Repositories.ADO.GenericRepository
{
    public class GenericRepositoryADO<T> : GenericRepositoryBase<T> where T : EntityBase, new()  
    {
        public GenericRepositoryADO(UnitOfWorkContext unitOfWorkContext)
            : base(unitOfWorkContext)
        {
        }

        //Overriding trigger mechanism
        public override T Insert(T obj)
        {
            return OnInsert(obj);
        }

        public override T OnInsert(T obj)
        {
            throw new NotImplementedException();
        }

        public override void OnUpdate(T obj)
        {
            throw new NotImplementedException();
        }

        public override void OnDelete(T obj)
        {
            throw new NotImplementedException();
        }

        public override T OnGetById(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> OnGetAll()
        {
            throw new NotImplementedException();
        }
    }
}
