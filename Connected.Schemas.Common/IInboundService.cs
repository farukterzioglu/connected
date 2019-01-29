namespace Connected.Schemas.Common
{
    using System.ServiceModel;

    /// <summary>
    /// Central Command service accessed from external system to push a transfer message
    /// </summary>
    [ServiceContract]
    public interface IInboundService
    {
        //IMessageStorage MessageStorage { get; set;}

        /// <summary>
        /// Process Message sent from external service consumers
        /// </summary>
        /// <param name="transferMessage">Message sent from external system</param>
        /// <param name="sourceAdapter"></param>
        /// <returns>Returns transfer message for the status of the message received</returns>
        [OperationContract]
        TransferMessage ProcessTransferMessage(TransferMessage transferMessage, string sourceAdapter);
    }
}
