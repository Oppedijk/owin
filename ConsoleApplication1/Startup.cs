using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.MicrosoftAccount;
using System.Security.Claims;

[assembly: OwinStartup(typeof(ConsoleApplication1.Startup))]

namespace ConsoleApplication1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ExternalCookie, //  == "ExternalCookie",
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
            });

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);
            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountAuthenticationOptions()
            {
                ClientId = "000000004C00563B", // per your setup through http://go.microsoft.com/fwlink/?LinkID=144070.
                ClientSecret = "2kjdvhubOjRcW28h5cNuXdatbDrVXKZz",
                //CallbackPath = new PathString("/signin-microsoft"), // default
                Provider = new MicrosoftAccountAuthenticationProvider()
                {
                    OnAuthenticated = (ctx) =>
                        Task.Run(() =>
                        {
                            ctx.OwinContext.Environment["server.User"] = new ClaimsPrincipal(ctx.Identity);

                        })
                }
            });

            app.Run(context =>
            {
//                if (!ClaimsPrincipal.Current.Identity.IsAuthenticated)
                
                if (context.Authentication.User == null || !context.Authentication.User.Identity.IsAuthenticated)
                {
                    context.Authentication.Challenge(new AuthenticationProperties
                    {
                        //RedirectUri = "http://dummy.oppedijk.com:12345/" // seems to be ignored
                    }, "Microsoft");
                    context.Set<int>("owin.ResponseStatusCode", 401);

                    return context.Response.WriteAsync("Redirecting...");
                }

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello " + ClaimsPrincipal.Current.Identity.Name + " from my OWIN App: " + DateTime.Now);
            });

            //app.Use(async (context, next) =>
            //{
            //    var user = context.Authentication.User;
            //    if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            //    {
            //        context.Authentication.Challenge();
            //        return;
            //    }
            //    await next();
            //});

            //app.Run(context =>
            //{
            //    // New code: Throw an exception for this URI path.
            //    if (context.Request.Path.Value == "/fail")
            //    {
            //        throw new Exception("Random exception");
            //    }

            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hello, world.");
            //});

        }
    }
}
