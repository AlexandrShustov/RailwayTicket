using Microsoft.Owin;
using Owin;
using RailwayTickets.WebUI;

[assembly: OwinStartup(typeof(Startup))]
namespace RailwayTickets.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
