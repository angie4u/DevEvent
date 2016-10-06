using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevEvent.Web.Startup))]
namespace DevEvent.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
