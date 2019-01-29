using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Repositories.ADO.GenericRepository
{

    public class UnitOfWorkADO : IUnitOfWork
    {
        //private Dictionary<Type, List<EntityBase< [int  string] >>> _identityMappingDictionary = null;

        private readonly UnitOfWorkContext _classContext;
        public UnitOfWorkADO(UnitOfWorkContext classContext = null)
        {
            _classContext = classContext ?? new UnitOfWorkContext();
        }

        public UnitOfWorkContext UnitOfWorkContext { get; private set; }

        public void Commit()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                //execute sql
            }
            throw new NotImplementedException();
        }

        public void Roolback()
        {
            throw new NotImplementedException();
        }

        public GenericRepositoryBase<T> GetRep<T>() where T : EntityBase, new()
        {
            return new GenericRepositoryADO<T>(_classContext);
        }

        public void Dispose()
        {
            //TODO : Clear identity mapping if active 
            throw new NotImplementedException();
        }
    }
}
