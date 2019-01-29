using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connected.Common;
using Connected.Configuration.WebApp.Plugins.Sample1.Controllers;
using Infrastructure.AspectOriented.Aspects;
using Infrastructure.CrossCutting.IocManager;
using Infrastructure.PluginFramework.Core;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Connected.Configuration.WebApp.Plugins.Sample1
{
    public class Sample1Plugin : IPlugin
    {
        private List<Type> _controllerList;
        public List<Type> ControllerTypes
        {
            get { return _controllerList ?? (_controllerList = new List<Type>()); }
            set { _controllerList = value; }
        }

        public string PluginAssemblyName
        {
            get { return "Sample1"; }
        }

        public string PluginName
        {
            get { return "Sample 1"; }
        }

        public Sample1Plugin()
        {
            ControllerTypes.Add(typeof(Sample1Controller));
            ControllerTypes.Add(typeof(Sample12Controller));

            //TODO : register to sub container per plugin
            IoCManager.Container.RegisterType<IPluginController, Sample1Controller>("Sample1Controller",
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PluginControllerAuthorizationBehavior>());

            IoCManager.Container.RegisterType<IPluginController, Sample12Controller>("Sample12Controller",
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<PluginControllerAuthorizationBehavior>());

            //_controllerList.Add(IoCManager.Instance.Resolve<IPluginController>("Sample1Controller"));
            //_controllerList.Add(IoCManager.Instance.Resolve<IPluginController>("Sample12Controller"));

        }
    }
}
