using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myBot.Startup))]
namespace myBot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
