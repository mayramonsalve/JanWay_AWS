using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWA.Core.Entities;
using JWA.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace JWA.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication(options=> {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options=>
                {
                    options.LoginPath = "/api/User/ExternalLoginGetRequest";
                })
                .AddGoogle(Options =>
                {
                    Options.ClientId = "840629081899-n0vfcm1pdq3lhmg790uj7v2gt2sdh6gf.apps.googleusercontent.com";
                    Options.ClientSecret = "-rCvwsiAQihHwNghSudMouQb";
                });
            services.AddIdentity<User, UserRole>();

            services.AddIdentityServer()
            .AddInMemoryIdentityResources(ServerConfiguration.IdentityResources)
            .AddInMemoryApiResources(ServerConfiguration.ApiResources)
            .AddInMemoryApiScopes(ServerConfiguration.ApiScopes)
            .AddInMemoryClients(ServerConfiguration.Clients)
            .AddTestUsers(ServerConfiguration.TestUsers)
            .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
