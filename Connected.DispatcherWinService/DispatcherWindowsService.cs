using System;
using System.ServiceProcess;
using Connected.Common;
using Connected.DAL.Configuration.Repositories;
using Connected.ModuleManager;
using Connected.Schemas;
using Infrastructure.CrossCutting.IocManager;

namespace Connected.DispatcherWinService
{
    public partial class DispatcherWindowsService : ServiceBase
    {
        //TODO : NInject this
        Connected.Common.LogManagement.ILoger _loger = new Connected.Common.LogManagement.EventLoger();

        //Host Inbound service
        private Dispatcher.DispatcherService _dispatcherService;

        public DispatcherWindowsService()
        {
            ConnectedModuleManager.Instance.RegisterModulesForIoC(ConnectedEnvironment.Production);

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //Select message queueu entities
                IMessageStorage messageStorage = IoCManager.Instance.ResolveIfRegistered<IMessageStorage>();// new FakeStorage(); //_messageStorage = new MessageQueueStorage();

                //Resolve Configuration DB Repository
                IConnectedConfDBRepository connectedConfDBRepository = IoCManager.Instance.ResolveIfRegistered<IConnectedConfDBRepository>();

                _dispatcherService = new Dispatcher.DispatcherService(messageStorage, connectedConfDBRepository);
                _dispatcherService.StartDispatcherService();

                _loger.WriteLog(
                    "Connected Dispatcher Service Started. ",
                    global::Common.Enums.LogType.Info, "Connected Dispatcher Service");
            }
            catch (Exception exception)
            {
                _loger.WriteLog(
                    "Connected Dispatcher Service couldn't started. Error message : " + exception.Message +
                    (!string.IsNullOrEmpty(exception.InnerException.Message)
                        ? ", Inner exception : " + exception.InnerException.Message
                        : ""),
                    global::Common.Enums.LogType.Error, "Connected Dispatcher Service");

                throw;
            }
        }

        protected override void OnStop()
        {
            _dispatcherService.Close();

            _loger.WriteLog(
                "Connected Dispatcher Service Stoped. ",
                global::Common.Enums.LogType.Info, "Connected Dispatcher Service");
        }
    }
}
