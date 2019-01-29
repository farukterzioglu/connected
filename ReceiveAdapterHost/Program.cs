using System;
using System.Configuration;
using System.ServiceModel;
using ReceiveAdapter;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;

namespace ReceiveAdapterHost
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //Get Url of Web Service
                ServicesSection servicesSection = (ServicesSection)ConfigurationManager.GetSection("system.serviceModel/services");
                ServiceEndpointElement endpoint = servicesSection.Services[0].Endpoints[0];
                string endpointAddress = endpoint.Address.ToString();

                //Open Web Service
                //ReceiveAdapterService service = new ReceiveAdapterService();
                ServiceHost serviceHost = new ServiceHost(typeof (ReceiveAdapterService));
                serviceHost.Open();

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Connect Web Services Started. To Stop, Press any key.....");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Service Url :" + endpointAddress);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in Hosting Receive Adapter Service");
                Console.WriteLine("Press any key to quit!.............");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
