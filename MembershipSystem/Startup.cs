using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MembershipSystem.Startup))]
namespace MembershipSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
