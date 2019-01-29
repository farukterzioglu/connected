using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connected.Configuration.WebApp.Plugins.Core
{
    public interface IPlugin
    {
        List<Type> ControllerTypes { get; set; }
        string PluginName { get; }
    }
}
