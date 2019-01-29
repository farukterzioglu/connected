using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Infrastructure.DataContextStorage
{
    public interface IDataContextStorageContainer<T>
    {
        T GetDataContext();
        void Store(T objectContext);
        void Clear();
    }
}
