using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DMS_thesis.Startup))]
namespace DMS_thesis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
