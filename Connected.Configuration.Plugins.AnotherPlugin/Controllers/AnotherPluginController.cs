using System;
using System.Web.Mvc;
using Infrastructure.PluginFramework.Core;

namespace Connected.Configuration.WebApp.Plugins.AnotherPlugin.Controllers
{
    //TODO : Inherit from base controller
    public class AnotherPluginController : Controller, IPluginController
    {
        public Type PluginType {
            get { return typeof(AnotherPlugin); }
        }

        public string AssemblyName {
            get { return "AnotherPlugin"; }
        }
        public string Layout { get; set; }

        public  string Title {
            get { return "Another Plugin Title"; }
            set { }
        }

        public ActionResult Index()
        {
            ViewBag.Layout = Layout;
            ViewBag.Title = Title;

            return View();
        }
    }
}
