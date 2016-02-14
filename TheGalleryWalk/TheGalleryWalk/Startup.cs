using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheGalleryWalk.Startup))]
namespace TheGalleryWalk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
