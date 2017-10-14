using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParentsBankProject.Startup))]
namespace ParentsBankProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
