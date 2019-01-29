using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.PluginFramework.Core;

namespace Connected.Configuration.WebApp.Plugins.AnotherPlugin
{
    //TODO : Inherit from base class
    public class AnotherPlugin : IPlugin
    {
        private List<IPluginController> _controllerList;
        public List<IPluginController> ControllerList
        {
            get { return _controllerList ?? (_controllerList = new List<IPluginController>()); }
            set { _controllerList = value; }
        }

        public List<Type> ControllerTypes { get; set; }

        public string PluginAssemblyName
        {
            get { return "AnotherPlugin"; }
        }

        private string _pluginName = "Another Plugin";
        public string PluginName
        {
            get { return _pluginName; }
        }
    }
}
