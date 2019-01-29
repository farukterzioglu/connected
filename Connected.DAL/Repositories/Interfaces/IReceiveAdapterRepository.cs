using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.DAL.Repositories.Interfaces
{
    public interface IReceiveAdapterRepository
    {
        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>Registraion id</returns>
        int RegisterItem(Activity activity);

        /// <summary>
        /// Stores data of an item
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>True/False regarding to succes of process</returns>
        bool ReadItem(Activity activity);

        /// <summary>
        /// Registers a new item and stores data of item
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        int RegisterReadItem(Activity activity);

        /// <summary>
        /// Get registration info of an item
        /// </summary>
        /// <param name="originalId"></param>
        /// <returns></returns>
        ItemRegistration GetItemRegistration(string originalId);
    }
}
