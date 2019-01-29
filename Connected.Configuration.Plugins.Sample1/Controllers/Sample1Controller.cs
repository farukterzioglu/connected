using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Helpers;
using Infrastructure.PluginFramework.Core;

namespace Connected.Configuration.WebApp.Plugins.Sample1.Controllers
{
    public class Sample1Controller : PluginControllerBase
    {
        public override Type PluginType
        {
            get { return typeof(Sample1Plugin); }
        }

        public override string Layout { get; set; }

        private string _title = "Sample 1 Title";
        public override string Title {
            get { return _title; }
            set { _title = value; }
        }

        protected override ActionResult OnIndex()
        {
            ViewBag.Layout = Layout;
            ViewBag.Title = Title;

            var modelIns = new Models.Sample1() { Value1 = "Test model value" };

            return View("Index", modelIns);
        }
    }
}
