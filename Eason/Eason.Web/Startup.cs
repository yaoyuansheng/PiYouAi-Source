using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Eason.Web.Startup))]
namespace Eason.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
