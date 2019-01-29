using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Connected.Configuration.WebApp.Plugins.Core {
    public static class PluginRouteHelper
    {
        public static void MapAreas(this RouteCollection routes, string url, string rootNamespace, string[] plugins) {
            Array.ForEach(plugins, plugin =>
            {
                Route route = new Route("{Plugin}/" + url, new MvcRouteHandler());
                route.Constraints = new RouteValueDictionary(new { plugin });
                string pluginNamespace = rootNamespace + ".Plugins." + plugin + ".Controllers";
                route.DataTokens = new RouteValueDictionary(new { namespaces = new string[] { pluginNamespace } });
                route.Defaults = new RouteValueDictionary(new { action = "Index", controller = "Home", id = "" });
                routes.Add(route);
            });
        }

        public static void MapRootArea(this RouteCollection routes, string url, string rootNamespace, object defaults) {
            Route route = new Route(url, new MvcRouteHandler());
            route.DataTokens = new RouteValueDictionary(new { namespaces = new string[] { rootNamespace + ".Controllers" } });
            route.Defaults = new RouteValueDictionary(new { area="root", action = "Index", controller = "Home", id = "" });
            routes.Add(route);
        }
    }
}
