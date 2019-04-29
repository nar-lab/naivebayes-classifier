using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NBayes.Startup))]
namespace NBayes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
