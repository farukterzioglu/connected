using System;
using System.Web.Mvc;

namespace Connected.Configuration.WebApp.Plugins.Core
{
    public interface IPluginController //<T> where T : IPlugin
    {
        Type PluginType { get;}
        string Layout { get; set; }
        string Title { get; set; }

        ActionResult Index();
    }
}
