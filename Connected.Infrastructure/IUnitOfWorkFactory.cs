using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Infrastructure
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        IUnitOfWork Create(bool forceNew);
    }

    public interface IUnitOfWork : IDisposable
    {
        void Commit(bool resetAfterCommit);
        void Undo();
    }
}
