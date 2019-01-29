using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Connected.Common;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core;
using Infrastructure.CrossCutting.IocManager;
using Infrastructure.PluginFramework.PluginManager;
using Infrastructure.WebApps.Common;

namespace Connected.Configuration.WebApp
{
    public class IoCConfig
    {
        public static void RegisterIoCEntities()
        {
            IoCManager.Instance.Register<IUnitOfWork, UnitOfWorkFake>();
        }

        public static void RegisterPreStartIoCEntities()
        {
            //User claim manager
            IoCManager.Instance.Register<IUserClaimsManager, UserClaimsManager>();
        }
    }

}