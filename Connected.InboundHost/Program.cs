using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Connected.Inbound;
using Connected.Infrastructure;
using Connected.Infrastructure.DataContextStorage;

namespace Connected.InboundHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO : NInject this two repositories
            //Select message queueu entities
            Schemas.IMessageStorage messageQueueStorage = new Connected.MessageStorage.MessageQueueStorage();

            //Select configuration repository
            Connected.DAL.Configuration.Repositories.IConnectedConfDBRepository connectedConfDBRepository = 
                new Connected.DAL.Configuration.Repositories.EF.ConnectedConfDBRepository();

            //TODO : NInject this
            Connected.Common.LogManagement.ILoger loger = new Connected.Common.LogManagement.EventLoger();

            //Host Inbound service
            InboundService service = new InboundService(messageQueueStorage, connectedConfDBRepository);
            try
            {
                ServiceHost serviceHost = new ServiceHost(service);
                serviceHost.Open();

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Connected Inbound Service Started. To Stop, Press any key.....");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Service Url :http://localhost:85/ConnectedRouterService");

                loger.WriteLog(
                    "Connected Inbound Service Started. Service Url :http://localhost:85/ConnectedRouterService",
                    global::Common.Enums.LogType.Info, "Connected Inbound Service");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in Hosting Connected Inbound Service");
                Console.WriteLine("Press any key to Quit!.............");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);

                loger.WriteLog(
                    "Connected Inbound Service couldn't started. Error message : " + exception.Message +
                    (!string.IsNullOrEmpty(exception.InnerException.Message)
                        ? ", Inner exception : " + exception.InnerException.Message
                        : ""),
                    global::Common.Enums.LogType.Error, "Connected Inbound Service");
            }
            Console.ReadLine();
        }
    }
}
