using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StartWatch.Startup))]
namespace StartWatch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
