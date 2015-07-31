using System;
using System.IdentityModel.Tokens;
using Microsoft.AspNet.Authentication.OAuthBearer;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;

namespace OAuthBearerAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

			app.UseOAuthBearerAuthentication(options =>
			{
				options.Authority = "https://login.windows.net/tushartest.onmicrosoft.com";
				options.Audience = "https://TusharTest.onmicrosoft.com/TodoListService-ManualJwt";
				options.AuthenticationScheme = OAuthBearerAuthenticationDefaults.AuthenticationScheme;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateLifetime = false
				};
			});

			// Add MVC to the request pipeline.
			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
