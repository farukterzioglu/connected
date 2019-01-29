using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Common;
using Connected.DAL.Configuration.Repositories;
using Connected.DAL.Configuration.Repositories.EF;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core;
using Connected.MessageStorage;
using Connected.MessageStorage.Fake;
using Connected.Schemas;
using Infrastructure.CrossCutting.IocManager;
using Microsoft.Practices.Unity;

namespace Connected.ModuleManager
{
    public enum ConnectedEnvironment
    {
        Test,
        Production
    }

    public class ConnectedModuleManager
    {
        private static Lazy<ConnectedModuleManager> _instance = new Lazy<ConnectedModuleManager>(() => new ConnectedModuleManager());
        public static ConnectedModuleManager Instance { get { return _instance.Value; } }

        static ConnectedModuleManager()
        {

        }

        public void RegisterModulesForIoC(ConnectedEnvironment environment)
        {
            if (environment == ConnectedEnvironment.Test)
            {
                IoCManager.Instance.Register<IMessageStorage, FakeStorage>(IoCLifeTimeType.PerThread);
                IoCManager.Instance.Register<IConnectedConfDBRepository, ConnectedConfDBRepository>(IoCLifeTimeType.PerThread);

                IoCManager.Instance.Register<Connected.Common.LogManagement.ILoger, Connected.Common.LogManagement.TextLoger>(IoCLifeTimeType.PerThread);
            }
            else if(environment == ConnectedEnvironment.Production)
            {
                IoCManager.Instance.Register<IMessageStorage, MessageQueueStorage>(IoCLifeTimeType.Singleton);
                IoCManager.Instance.Register<IConnectedConfDBRepository, ConnectedConfDBRepository>(IoCLifeTimeType.Singleton);

                IoCManager.Instance.Register<Connected.Common.LogManagement.ILoger, Connected.Common.LogManagement.TextLoger>(IoCLifeTimeType.PerThread);
            }
        }
    }
}
