using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Agent.Startup))]
namespace Agent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
