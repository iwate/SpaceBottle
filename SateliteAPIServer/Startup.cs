using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SateliteAPIServer.Startup))]
namespace SateliteAPIServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
