using System;
using System.Web.Mvc;
using Infrastructure.PluginFramework.Core;

namespace Connected.Configuration.WebApp.Plugins.Sample1.Controllers
{
    public class Sample12Controller : PluginControllerBase
    {
        public override Type PluginType {
            get { return typeof (Sample1Plugin); }
        }
        public override string Layout { get; set; }

        private string _title = "Sample 12 Title";
        public override string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        protected override ActionResult OnIndex()
        {
            ViewBag.Layout = Layout;
            ViewBag.Title = Title;

            var modelIns = new Models.Sample1() { Value1 = "Test model value" };

            return View( "Index",modelIns);
        }
    }
}
