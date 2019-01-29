using System;
using System.Web.Mvc;

namespace Connected.Configuration.WebApp.Plugins.Core
{
    public abstract class PluginControllerBase : Controller, IPluginController
    {
        public abstract Type PluginType { get; }
        public abstract string Layout { get; set; }
        public abstract string Title { get; set; }

        public ActionResult Index()
        {
            return OnIndex();
        }
        protected abstract ActionResult OnIndex();
    }
}
