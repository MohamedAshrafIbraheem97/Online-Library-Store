using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(libraryStoreFinal.Startup))]
namespace libraryStoreFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
