using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Schemas
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IRouteFinderService
    {
        IMessageStorage MessageStorage { get; set; }
        //IConnectedConfDBRepository ConnectedConfDBRepository { get; set; }

        //TODO : Returns  task for testing, get rid of it 
        [OperationContract]
        Task StartRouteFinderService();

        /// <summary>
        ///  evaluate the messages against a list of registered subscribers and any subscription criteria
        /// </summary>
        [OperationContract]
        bool RouteMessages();

        [OperationContract] 
        void Close();
    }
}
