using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Core;

namespace Connected.DAL.Configuration.Repositories.Fake
{
    public class UnitOfWorkFake : IUnitOfWork
    {
        private readonly UnitOfWorkContext _unitOfWorkContext;

        public UnitOfWorkContext UnitOfWorkContext
        {
            get { return _unitOfWorkContext; }
        }

        public UnitOfWorkFake()
        {
            _unitOfWorkContext = new UnitOfWorkContext()
            {
                IdentityMappingActive = false,
                IdentityMappingRecordLimit = 0
            };
        }

        public UnitOfWorkFake(UnitOfWorkContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }

        public void Commit()
        {
            
        }

        public void Roolback()
        {
            throw new NotImplementedException();
        }

        public GenericRepositoryBase<T> GetRep<T>() where T : EntityBase, new()
        {
            return new GenericConfigurationRepositoryFake<T>(UnitOfWorkContext);
        }

        public void Dispose()
        {
        }
    }
}
