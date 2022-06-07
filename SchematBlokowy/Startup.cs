using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchematBlokowy.Startup))]
namespace SchematBlokowy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
