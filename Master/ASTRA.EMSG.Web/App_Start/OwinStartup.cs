using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using ASTRA.EMSG.Business.Models.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(ASTRA.EMSG.Web.App_Start.OwinStartup))]

namespace ASTRA.EMSG.Web.App_Start
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/auth/login"),
                CookieName = "EMSG",
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromHours(4)
            });
        }
    }
}
