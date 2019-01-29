    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
    using Connected.Common;
    using Infrastructure.CrossCutting.IocManager;
    using Infrastructure.PluginFramework.Core;
    using Infrastructure.PluginFramework.PluginManager;

namespace Connected.Configuration.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("hello.htm"); //ignore the specific HTML start page
            //routes.IgnoreRoute(""); //to ignore any default root requests

            //routes.MapRoute(
            //    name: "Module",
            //    url: "Module/{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            ////TODO : Get plugin list from manager
            ////Reference for extension methods : http://haacked.com/archive/2008/11/04/areas-in-aspnetmvc.aspx/
            //routes.MapAreas("{controller}/{action}/{id}",
            //    "Connected.Configuration.WebApp",
            //    new[] {"AnotherPlugin", "Sample1", "Sample12"});

            //routes.MapRootArea("{controller}/{action}/{id}",
            //    "Connected.Configuration.WebApp",
            //    new {controller = "Home", action = "Index", id = ""});


            ////TODO : Just an idea, test this 
            ////TODO : move this to plugin manager ?
            //var pluginList = PluginManager.Instance.GetPluginNames();
            //foreach (var pluginName in  pluginList)
            //{
            //    try
            //    {
            //        routes.MapRoute(
            //            name: pluginName,
            //            url: pluginName + "/{controller}/{action}/{id}",
            //            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //            , namespaces: new[] { pluginName });
            //    }
            //    catch (Exception)
            //    {
            //        continue;
            //    }
            //}
        }
    }
}
