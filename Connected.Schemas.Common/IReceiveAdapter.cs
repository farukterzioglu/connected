using System.ServiceModel;

namespace Connected.Schemas.Common
{
    [ServiceContract]
    public interface IReceiveAdapter
    {
        /// <summary>
        /// This method will process the incoming TransferMessage and optionally it can send the response back to the caller
        /// </summary>
        /// <param name="transferMessage">The incoming TransferMessage</param>
        [OperationContract]
        void ProcessMessage(TransferMessage transferMessage);
    }
}
