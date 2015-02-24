using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Security.Cookies;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using OAuthBearerClient.Models;
using Microsoft.AspNet.Security;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Microsoft.AspNet.Security.OpenIdConnect;

#if ASPNET50
using OAuthBearerClient.Logging;
#endif

namespace OAuthBearerClient
{
    public class Startup
    {
		private static string clientId = "ba7651c2-53c2-442a-97c2-3d60ea42f403";
		private static string appKey = "ZpmbkS5HXK3Zpl5rC8nO/0vDTW7TZI0qjqDdJgKqihk=";
		private static string authority = "https://login.windows.net/tushartest.onmicrosoft.com";

		public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF services to the services container.
            services.AddEntityFramework(Configuration)
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>();

            // Add Identity services to the services container.
            //services.AddIdentity<ApplicationUser, IdentityRole>(Configuration)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add MVC services to the services container.
            services.AddMvc();

			// Uncomment the following line to add Web API servcies which makes it easier to port Web API 2 controllers.
			// You need to add Microsoft.AspNet.Mvc.WebApiCompatShim package to project.json
			// services.AddWebApiConventions();

			services.Configure<ExternalAuthenticationOptions>(options =>
			{
				options.SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType;
			});

		}

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
			// Configure the HTTP request pipeline.
			// Add the console logger.
#if ASPNET50
			loggerfactory.AddEventSourceLogger();
#else
			loggerfactory.AddConsole();
#endif
			// Add the following to the request pipeline only in development environment.
			if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

			// Add cookie-based authentication to the request pipeline.
			//app.UseIdentity();

			app.UseCookieAuthentication(options => { });

			app.UseOpenIdConnectAuthentication(options =>
			{
				options.ClientId = "ba7651c2-53c2-442a-97c2-3d60ea42f403";
				options.Authority = "https://login.windows.net/tushartest.onmicrosoft.com";
				options.Notifications = new OpenIdConnectAuthenticationNotifications()
				{
					AuthorizationCodeReceived = async (context) =>
					{
						var code = context.Code;

						ClientCredential credential = new ClientCredential(clientId, appKey);
						AuthenticationContext authContext = new AuthenticationContext(authority);
						AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(code, new Uri("http://localhost:42023"), credential);
					}
				};
            });

			// Add MVC to the request pipeline.
			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
