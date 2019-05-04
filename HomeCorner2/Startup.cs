using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeCorner2.Startup))]
namespace HomeCorner2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
