using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigData.Startup))]
namespace BigData
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
