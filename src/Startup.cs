using Blazor5Auth.Server.Data;
using Blazor5Auth.Server.Extensions;
using Blazor5Auth.Server.Models;
using Blazor5Auth.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using Features.Account;
using Features.Base;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Blazor5Auth.Server.Services;
using MongoFramework;
using MongoFramework.AspNetCore.Identity;
using System.Text.Json;
using VueCliMiddleware;

namespace Blazor5Auth.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            services.AddMongoDbContext<ApplicationDbContext>(o =>
                o.ConnectionString = Configuration.GetConnectionString("DefaultConnection"));

            services.AddMongoIdentity<ApplicationUser, MongoIdentityRole, ApplicationDbContext>(options => options.SignIn.RequireConfirmedAccount = true);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });

            // This allows customization of the result when model binding fails on ApiControllers
            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(new BaseResult().WithErrors(context.ModelState));
                    };
                })
                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = true;
                    fv.RegisterValidatorsFromAssemblyContaining<BaseResult>();
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            //services.AddRazorPages()

            //email services
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            services.AddTransient<IJwtHelper, JwtHelper>();
            services.AddScoped<IUserAccessor, UserAccessor>();

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _ = CommandLine.Arguments.TryGetOptions(System.Environment.GetCommandLineArgs(), false, out string mode, out ushort port, out bool https);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (https) app.UseHttpsRedirection();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
//                endpoints.MapRazorPages();
                endpoints.MapControllers();
                // endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {

                    // run npm process with client app
                    if (mode == "start") {
                        spa.UseVueCli(npmScript: "serve", port: port, forceKill: true, https: https);
                    }

                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead,
                    // app should be already running before starting a .NET client:
                    // run npm process with client app
                    if (mode == "attach") {
                        spa.UseProxyToSpaDevelopmentServer($"{(https ? "https" : "http")}://localhost:{port}"); // your Vue app port
                    }
                }
            });


            SeedData.Initialize(app.ApplicationServices);
        }
    }

}

