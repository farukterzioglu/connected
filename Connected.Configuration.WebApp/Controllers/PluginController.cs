using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Connected.Common;
using Connected.Configuration.WebApp.ViewModel;
using Infrastructure.AspectOriented.Aspects;
using Infrastructure.CrossCutting.IocManager;
using Infrastructure.PluginFramework.Core;
using Infrastructure.PluginFramework.PluginManager;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;

// ReSharper disable once CheckNamespace
namespace Connected.Configuration.WebApp.Root.Controllers
{
    public class PluginController : Controller
    {
        private void AddPlugin(string pluginName)
        {
            try
            {
                IPlugin plugin = IoCManager.Instance.ResolveIfRegistered<IPlugin>(pluginName);

                var temp = IoCManager.Container.Resolve(typeof (IPlugin), pluginName);
                
                if (plugin != null)
                    _pluginList.Add(plugin);
            }
            catch (NotRegisteredException ex)
            {
                return;
            }
        }

        private readonly List<IPlugin> _pluginList = new List<IPlugin>();
        public PluginController()
        {
            List<string> pluginList = PluginManager.Instance.GetPluginNames();
            pluginList.ForEach(AddPlugin);

            //Testing ->
            List<PluginControllerBase> pluginControllers = new List<PluginControllerBase>();
            foreach (IPlugin plugin in _pluginList)
            {
                if (plugin.ControllerTypes == null) continue;
                foreach (Type pluginControllerType in plugin.ControllerTypes)
                {
                    //IPluginController controller = Activator.CreateInstance(pluginControllerType) as IPluginController;
                    //if(controller == null) continue;

                    try
                    {
                        //TODO : User sub container to resolve plugin specific controller, same controller name for two different plugin cause problem
                        var pluginController = IoCManager.Container.Resolve<IPluginController>(pluginControllerType.Name);

                        var title = pluginController.Title;
                    }
                    catch (NotAuthorizedException ex )
                    {
                        //User doesn't have claim for this type, pass this one
                    }
                }
            }
        }

        public ActionResult Plugins()
        {
            var list = RouteTable.Routes.ToList();        
            
            PluginViewModel pluginViewModel = new PluginViewModel();
            pluginViewModel.PluginList = _pluginList;

            return View(pluginViewModel);
        }
    }
}