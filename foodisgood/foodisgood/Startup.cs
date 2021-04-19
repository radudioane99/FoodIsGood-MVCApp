using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(foodisgood.Startup))]
namespace foodisgood
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
