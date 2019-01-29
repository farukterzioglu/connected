using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using Connected.Common;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core;
using System.IO;
using System.Web.Compilation;
using System.Web.Hosting;
using Infrastructure.CrossCutting.IocManager;
using Infrastructure.PluginFramework.PluginManager;
using Infrastructure.WebApps.Common;
using WebGrease.Css.Extensions;

namespace Connected.Configuration.WebApp
{
    public static class Initializer
    {
        public static void Initialize()
        {
            IoCConfig.RegisterPreStartIoCEntities();
            
            //TODO : Create folders if not exist
            //Set plugin
            PluginManager.Instance.SetContext(new PluginManagerContext(
                IoCManager.Instance.ResolveIfRegistered<IUserClaimsManager>(),
                new DirectoryInfo(HostingEnvironment.MapPath("~/plugins")),
               new DirectoryInfo(HostingEnvironment.MapPath("~/plugins/temp"))));

            PluginManager.Instance.InitializePlugins();
        }
    }
}