using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Connected.Configuration.WebApp.Startup))]
namespace Connected.Configuration.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
