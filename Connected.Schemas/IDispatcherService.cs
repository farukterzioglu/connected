using System.Threading.Tasks;

namespace Connected.Schemas
{
    //using System.ServiceModel;

    //[ServiceContract]
    public interface IDispatcherService
    {
        IMessageStorage MessageStorage { get; set;}
        //IConnectedConfDBRepository ConnectedConfDBRepository { get; set; }

        /// <summary>
        /// TODO : Fill this
        /// </summary>
        //[OperationContract]
        Task StartDispatcherService();

        /// <summary>
        /// TODO : Fill this
        /// </summary>
        //[OperationContract]
        bool MoveMessagesToMessagesRouterFinder();

        /// <summary>
        /// TODO : Fill this
        /// </summary>
        //[OperationContract]
        void PushMessages();

        //[OperationContract]
        void Close();
    }
}
