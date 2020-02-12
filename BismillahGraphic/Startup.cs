using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BismillahGraphic.Startup))]
namespace BismillahGraphic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
