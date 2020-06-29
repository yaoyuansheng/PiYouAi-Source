using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Eason.Admin.Startup))]
namespace Eason.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configure(app);
        }
    }
}
