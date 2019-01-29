using System;
using System.Threading;
using Schemas.SendAdapter;

namespace SendAdapter
{
    class Program
    {
        static void Main(string[] args)
        {
            ISendAdapter sendAdapter = new SendAdapterClass();

            while (true)
            {
                var dataReadOk = sendAdapter.ReadData();
                if (dataReadOk)
                    Console.WriteLine("New values have been read. " + DateTime.Now);
                else
                    Console.WriteLine("Couldn't read data.");

                //TODO : Read Interval from appConfig
                Thread.Sleep(10000);
            }

            //var tryNtimes = 0;
            //do
            //{
            //    bool registered = false;

            //    //Check registration, if not registered, register it. 
            //    if (!sendAdapter.Registered)  sendAdapter.Register();
            //    else registered = sendAdapter.Registered;

            //    if (registered)
            //    {
            //        Console.WriteLine("Item registered.");

            //        while (true)
            //        {
            //            var dataReadOk = sendAdapter.ReadData();
            //            if (dataReadOk)
            //                Console.WriteLine("New values have been read. " + DateTime.Now);
            //            else
            //                Console.WriteLine("Couldn't read data.");

            //            //TODO : Read Interval from appConfig
            //            Thread.Sleep(1000);
            //        }
            //    }

            //    Console.WriteLine("Couldn't register new item. Try : " + ++tryNtimes);
            //    //tryNtimes++;
            //} while (tryNtimes < 5);
            
            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        sendAdapter.ProcessData();

            //        //TODO : Read Interval from appConfig
            //        Thread.Sleep(3000);
            //    }
            //});
        }
    }
}
