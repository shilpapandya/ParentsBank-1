using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PB.Startup))]
namespace PB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
