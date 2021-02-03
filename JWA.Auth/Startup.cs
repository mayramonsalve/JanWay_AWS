using FluentValidation;
using FluentValidation.AspNetCore;
//using JWA.Auth.Interface;
using JWA.Auth.Models;
//using JWA.Auth.Repository;
using JWA.Core.DTOs;
using JWA.Core.Services;
using JWA.Infrastructure.Data;
using JWA.Infrastructure.Repositories;
using JWA.Infrastructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreInterface = JWA.Core.Interfaces;

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
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("JWA"))
            );
            services.AddDbContext<JWAContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("JWA"))
            );
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;

                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc()
                .AddFluentValidation();
            services.AddTransient<IValidator<InviteDtos>, InviteValidator>();
            services.AddTransient<IValidator<EmailDto>, EmailValidators>();
            services.AddTransient<IValidator<ConfirmEmailDto>, ConfirmEmailValidator>();
            services.AddTransient<IValidator<ChangeUserPasswordDto>, ChangeUserPasswordValidadtor>();
            services.AddTransient<IValidator<RecoverPasswordDto>, RecoverPasswordValidator>();
            services.AddTransient<IValidator<EditProfileDto>, EditProfileValidator>();
            services.AddTransient<IValidator<SignInDto>, SignInValidator>();

            services.AddControllers();
            //services.AddScoped<IOrganizationServices, OrganizationServices>();
            services.AddTransient<CoreInterface.ISendEmailService, SendEmailService>();
            services.AddTransient<CoreInterface.IInviteService, InviteService>();
            services.AddTransient<CoreInterface.IInviteRepository, InviteRepository>();
            services.AddTransient<CoreInterface.IFacilityService, FacilityService>();
            services.AddTransient<CoreInterface.IFacilityRepository, FacilityRepository>();
            services.AddTransient<CoreInterface.IOrganizationService, OrganizationService>();
            services.AddTransient<CoreInterface.IOrganizationRepository, OrganizationRepository>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "840629081899-n0vfcm1pdq3lhmg790uj7v2gt2sdh6gf.apps.googleusercontent.com";
                    options.ClientSecret = "-rCvwsiAQihHwNghSudMouQb";
                });
            AddSwagger(services);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JanWay Auth Module", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                //c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
