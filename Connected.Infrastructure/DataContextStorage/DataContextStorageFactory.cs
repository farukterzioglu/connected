using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Connected.Infrastructure.DataContextStorage
{
    public static class DataContextStorageFactory<T> where T : class
    {
        private static IDataContextStorageContainer<T> _dataContextStorageContainer;

        public static IDataContextStorageContainer<T> CreateStorageContainer()
        {
            if (_dataContextStorageContainer == null)
            {
                if (HttpContext.Current == null)
                {
                    _dataContextStorageContainer = new ThreadDataContextStorageContainer<T>();
                }
                else
                {
                    _dataContextStorageContainer = new HttpDataContextStorageContainer<T>();
                }
            }
            return _dataContextStorageContainer;
        }
    }
}
