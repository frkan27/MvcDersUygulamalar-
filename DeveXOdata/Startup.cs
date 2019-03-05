using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeveXOdata.Startup))]
namespace DeveXOdata
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
