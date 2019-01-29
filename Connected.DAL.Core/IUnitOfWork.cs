using System;

namespace Connected.DAL.Core
{
    public class UnitOfWorkContext
    {
        public bool IdentityMappingActive = false;
        public int? IdentityMappingRecordLimit = null;
    }

    public interface IUnitOfWork : IDisposable
    {
        UnitOfWorkContext UnitOfWorkContext { get; }

        void Commit();
        void Roolback();
        GenericRepositoryBase<T> GetRep<T>() where T : EntityBase, new();    
    }
}
