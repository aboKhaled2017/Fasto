using Fastdo.API.Hubs;
using Fastdo.Core;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fastdo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SysDbContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("smarterFastdo"),
                    builder =>
                    {
                        builder.MigrationsAssembly("Fastdo.API");
                    });
                //options.UseSqlServer(Configuration.GetConnectionString("smarterFastdo"), builder =>
                //{
                //    builder.MigrationsAssembly("Fastdo.API");
                //});

            });
            services.AddControllersWithViews();
            services._AddRepositories();
            services
                .AddIdentity<AppUser, IdentityRole>()
                 .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SysDbContext>();

            services._AddAutoMapper();
            services._AddSystemServices();
            services._AddSystemAuthentication();
            services._AddSystemAuthorizations();

            services.AddCors(options =>
            {
                options.AddPolicy(Variables.corePolicy, builder =>
                {
                //    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(_ => true);
                    builder.AllowCredentials();
                    builder.WithExposedHeaders(Variables.X_PaginationHeader);
                });
            });
            services.Configure<IdentityOptions>(op =>
            {
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireLowercase = false;
                op.Password.RequireDigit = false;

            });
            services.Configure<DataProtectionTokenProviderOptions>(op =>
            {
                op.TokenLifespan = TimeSpan.FromDays(1);
            });
            services._AddSwaggr();
            services.AddSignalR(c =>
            {
                c.EnableDetailedErrors = true;
            });
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new MyIdProvider());
            //services._AddGraphQlServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            app.UseCors(Variables.corePolicy);
            app._UserSwagger();
            app._UseExceptions(env);
            //app.UseHsts();
            //app.UseHttpsRedirection();
            app._UseServicesStarter(serviceProvider);
            //app._UseMyDbConfigStarter(env);
            //app._UseQraphQl();
            //app._useCustomFunctionToBeImplemented(env);/*disable it*/
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePages(context =>
            {
                if (context.HttpContext.Request.Path.Value.Contains("/AdminPanel", StringComparison.OrdinalIgnoreCase) &&
                    !context.HttpContext.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    var response = context.HttpContext.Response;
                    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                        response.StatusCode == (int)HttpStatusCode.Forbidden)
                        response.Redirect("/AdminPanel/Auth/Signin");
                }
                return Task.CompletedTask;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=AdminPanel}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<TechSupportMessagingHub>("/hub/techsupport");

            });
        }

    }
}

