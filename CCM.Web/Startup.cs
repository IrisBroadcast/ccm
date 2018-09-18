using System;
using System.Net;
using CCM.Web;
using CCM.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CCM.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseCors(CorsOptions.AllowAll);

            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true };
            app.MapSignalR(hubConfiguration);

            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(15);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(5);

            ServicePointManager.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallback;
        }


    }
}

