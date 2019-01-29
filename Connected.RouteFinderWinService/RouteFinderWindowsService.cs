using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Connected.Common;
using Connected.DAL.Configuration.Repositories;
using Connected.Schemas;
using Infrastructure.CrossCutting.IocManager;

namespace Connected.RouteFinderWinService
{
    public partial class RouteFinderWindowsService : ServiceBase
    {
        //TODO : NInject this
        Connected.Common.LogManagement.ILoger _loger = new Connected.Common.LogManagement.EventLoger();

        //Host Inbound service
        private Connected.RouteFinder.RouteFinderService _routeFinderService;

        public RouteFinderWindowsService()
        {
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

                _routeFinderService = new RouteFinder.RouteFinderService(messageStorage, connectedConfDBRepository);

                _routeFinderService.StartRouteFinderService();

                _loger.WriteLog(
                    "Connected Route Finder Service Started. ",
                    global::Common.Enums.LogType.Info, "Connected Route Finder Service");
            }
            catch (Exception exception)
            {
                _loger.WriteLog(
                    "Connected Route Finder Service couldn't started. Error message : " + exception.Message +
                    (!string.IsNullOrEmpty(exception.InnerException.Message)
                        ? ", Inner exception : " + exception.InnerException.Message
                        : ""),
                    global::Common.Enums.LogType.Error, "Connected Route Finder Service");

                throw;
            }
        }

        protected override void OnStop()
        {
            _routeFinderService.Close();

            _loger.WriteLog(
                "Connected Route Finder Service Stoped. ",
                global::Common.Enums.LogType.Info, "Connected Route Finder Service");
        }
    }
}
