using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.Interfaces;
using Connected.Infrastructure;

namespace Connected.DAL.Repositories.EntityFramework
{
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return Create(false);
        }

        public IUnitOfWork Create(bool forceNew)
        {
            return new EFUnitOfWork(forceNew);
        }
    }

    public class EFUnitOfWork : IUnitOfWork
    {
        //TODO : List all repositories
        public IActivityRepository activityRepository;

        public EFUnitOfWork(bool forceNewContext)
        {
            //TODO : List all repositories
            activityRepository = new ActivityRepository();

            if (forceNewContext)
            {
                DataContextFactory.Clear();
            }
        }

        public void Dispose()
        {
            DataContextFactory.GetDataContext().SaveChanges();
        }

        public void Commit(bool resetAfterCommit)
        {
            DataContextFactory.GetDataContext().SaveChanges();
            if (resetAfterCommit)
            {
                DataContextFactory.Clear();
            }
        }

        public void Undo()
        {
            DataContextFactory.Clear();
        }
    }
}
