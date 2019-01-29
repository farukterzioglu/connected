using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Connected.Common;
using Connected.DAL.Configuration.Repositories;
using Connected.Inbound;
using Connected.ModuleManager;
using Infrastructure.CrossCutting.IocManager;

namespace Connected.InboundWinService
{
    public partial class InboundWindowsService : ServiceBase
    {
        //TODO : NInject this
        Connected.Common.LogManagement.ILoger _loger = new Connected.Common.LogManagement.EventLoger();

        //Host Inbound service
        private InboundService _service;
        private ServiceHost _serviceHost;

        public InboundWindowsService()
        {
            //Register modules
            ConnectedModuleManager.Instance.RegisterModulesForIoC(ConnectedEnvironment.Production);

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ////Resolve IMessageStorage from IocManager
                //Schemas.IMessageStorage messageQueueStorage = IoCManager.Instance.Resolve<Schemas.IMessageStorage>();
                //if (messageQueueStorage == null)
                //    throw new NullReferenceException("Couldn't assign IMessageStorage");

                ////Resolve Configuration DB Repository
                //IConnectedConfDBRepository connectedConfDBRepository = 
                //    IoCManager.Instance.Resolve<IConnectedConfDBRepository>();

                _service = IoCManager.Instance.ResolveIfRegistered<InboundService>();
                //_service = new InboundService(messageQueueStorage, connectedConfDBRepository);

                _serviceHost = new ServiceHost(_service);
                _serviceHost.Open();

                _loger.WriteLog(
                    "Connected Inbound Service Started. ",
                    global::Common.Enums.LogType.Info, "Connected Inbound Service");
            }
            catch (Exception exception)
            {
                _loger.WriteLog(
                    "Connected Inbound Service couldn't started. Error message : " + exception.Message +
                    (!string.IsNullOrEmpty(exception.InnerException.Message)
                        ? ", Inner exception : " + exception.InnerException.Message
                        : ""),
                    global::Common.Enums.LogType.Error, "Connected Inbound Service");

                throw exception;
            }

        }

        protected override void OnStop()
        {
            if (_serviceHost == null) return;

            _serviceHost.Close();
            _serviceHost = null;

            //TODO : Log : Service stopped
        }
    }
}
