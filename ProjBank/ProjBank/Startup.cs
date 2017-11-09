using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjBank.Startup))]
namespace ProjBank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
