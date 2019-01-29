using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.PluginFramework.Core;

namespace Connected.Configuration.WebApp.ViewModel
{
    public class PluginViewModel
    {
        public IEnumerable<IPlugin> PluginList;
    }
}