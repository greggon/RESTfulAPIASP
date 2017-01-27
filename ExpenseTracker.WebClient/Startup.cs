using ExpenseTracker.WebClient.Helpers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(ExpenseTracker.WebClient.Startup))]

namespace ExpenseTracker.WebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
                                        {
                                            AuthenticationType = "Cookies"
                                        });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                                               {
                                                   ClientId = "mvc",
                                                   Authority = ExpenseTrackerConstants.IdSrv,
                                                   RedirectUri = ExpenseTrackerConstants.ExpenseTrackerClient,
                                                   SignInAsAuthenticationType = "Cookies",
                                                   ResponseType = "code id_token",
                                                   Scope = "openid profile",

                                                   Notifications = new OpenIdConnectAuthenticationNotifications()
                                                                   {
                                                                       MessageReceived = async n =>
                                                                                         {
                                                                                             EndpointAndTokenHelper
                                                                                                 .DecodeAndWrite(
                                                                                                     n.ProtocolMessage
                                                                                                         .IdToken);
                                                                                         }
                                                                   }
                                               });
        }
 
    }
}