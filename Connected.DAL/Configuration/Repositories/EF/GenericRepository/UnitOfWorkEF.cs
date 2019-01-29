using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Repositories.EF.GenericRepository
{
    public class UnitOfWorkEF : IUnitOfWork,IDisposable
    {
        private readonly UnitOfWorkContext _classContext;
        public UnitOfWorkEF(UnitOfWorkContext classContext = null)
        {
            _classContext = classContext ?? new UnitOfWorkContext();
        }

        public UnitOfWorkContext UnitOfWorkContext { get; private set; }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Roolback()
        {
            throw new NotImplementedException();
        }

        public GenericRepositoryBase<T> GetRep<T>() where T : EntityBase, new()
        {
            return new GenericRepositoryEF<T>(_classContext);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
