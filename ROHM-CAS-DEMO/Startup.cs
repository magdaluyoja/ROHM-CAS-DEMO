using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ROHM_CAS_DEMO.Startup))]
namespace ROHM_CAS_DEMO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
