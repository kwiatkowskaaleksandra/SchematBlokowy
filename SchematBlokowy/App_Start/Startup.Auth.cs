using Microsoft.AspNet.SignalR;
using Owin;

namespace SchematBlokowy
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var hubConfiguration = new HubConfiguration();
            //hubConfiguration.EnableJavaScriptProxies = false;
            app.MapSignalR(hubConfiguration);
            //GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}