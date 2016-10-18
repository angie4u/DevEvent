using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DevEvent.Mobile.Startup))]

namespace DevEvent.Mobile
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}