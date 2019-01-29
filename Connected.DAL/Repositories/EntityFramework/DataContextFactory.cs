using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Connected.Infrastructure.DataContextStorage;

namespace Connected.DAL.Repositories.EntityFramework
{
    public static class DataContextFactory
    {
        public static void Clear()
        {
            var dataContextStorageContainer =
                    DataContextStorageFactory<ConnectedEntities>.CreateStorageContainer();
            dataContextStorageContainer.Clear();
        }

        public static ConnectedEntities GetDataContext()
        {
            var dataContextStorageContainer =
                 DataContextStorageFactory<ConnectedEntities>.CreateStorageContainer();
            var contactManagerContext = dataContextStorageContainer.GetDataContext();
            if (contactManagerContext == null)
            {
                contactManagerContext = new ConnectedEntities();
                dataContextStorageContainer.Store(contactManagerContext);
            }
            return contactManagerContext;
        }
    }
}
