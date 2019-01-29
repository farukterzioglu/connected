using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.DAL.Repositories.Interfaces;
using Connected.Infrastructure;

namespace Connected.DAL.Repositories.ADONET
{
    public class UnitOfWork
    {
        private bool isCommited = false;
        public Repository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>();
        }

        public UnitOfWork()
        {
        }

        public void Dispose()
        {
            if (!isCommited)
                Commit();
            
            //TODO : Dispose 
        }

        public void Commit()
        {
            //TODO : Commit changes 
        }

        public void Undo()
        {
            //TODO : Rollback changes 
        }
    }
}
