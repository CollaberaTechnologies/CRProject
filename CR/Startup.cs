using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CR.Startup))]
namespace CR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
