using System;
using System.Collections.Generic;
using System.Reflection;
using Connected.Common.SerializationHelper;
using Connected.DAL;
using Connected.Schemas;
using Connected.Schemas.Common;
using Ninject;
using ReceiveAdapterBussinessLayer;
using Schemas;

namespace ReceiveAdapter
{
    public class ReceiveAdapterService : IReceiveAdapter
    {
        private readonly ReceiveAdapterBL _bl;

        public ReceiveAdapterService()
        {
            var registrationKey = System.Configuration.ConfigurationManager.AppSettings["registrationKey"] ?? null;
            if (registrationKey == null)
            {
                //TODO : Handle error???
                throw new NullReferenceException("Couldn't find 'registrationKey' in AppSettings");
            }
            //Set data access layer... 
            #region ... with manuel dependency injection 
            //var dataContext = DAL.Repositories.EF.DataContextFactory.GetDataContext();
            //dataContext.Configuration.LazyLoadingEnabled = false;
            //var dal = new ReceiveAdapterDALEF(dataContext);
            //bl = new ReceiveAdapterBL.ReceiveAdapterBL(dal);
            #endregion

            #region ... with Ninject from assembly -> 
            IKernel kernel = new StandardKernel(new Bindings("EF"));
            kernel.Load(Assembly.GetExecutingAssembly());
            var dal = kernel.Get<Connected.DAL.Repositories.Interfaces.IReceiveAdapterRepository>();
            _bl = new ReceiveAdapterBL(dal, registrationKey);
            #endregion

            #region ... with Ninject dynamically
            //TODO : Find a way to send constructer parameters, check 'Inject'
            //IKernel kernel = new StandardKernel(new Bindings("EF"));
            //_bl = kernel.Get<ReceiveAdapterBL>();
            #endregion
        }

        public void ProcessMessage(TransferMessage transferMessage)
        {
            //Parse message content
            if (transferMessage.Body != null)
            {
                try
                {
                    var xmlSource = transferMessage.Body.OuterXml;
                    var lstActivity = SerializationHelper.DeSerializeFromString<List<Activity>>(xmlSource);

                    //Send to BussinessLayer
                    _bl.ReceiveActivityList(lstActivity);
                }
                catch (Exception ex)
                {
                    //TODO : Log exception
                    throw ex;
                }
            }
        }
    }
}
