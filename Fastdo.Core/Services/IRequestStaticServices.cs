using Fastdo.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Fastdo.Core.Services
{
    public static class RequestStaticServices
    {
        private static IHttpContextAccessor _httpContextAccessor { get; set; }
        private static IServiceProvider _serviceProvider { get; set; }
        private static IServiceScope _serviceScope { get; set; }
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }
        public static HttpContext GetCurrentHttpContext()
        {
            return _serviceScope.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;
        }
        public static SysDbContext GetDbContext()
        {
            return _serviceScope.ServiceProvider.GetService<SysDbContext>();
        }
        public static IWebHostEnvironment GetHostingEnv()
        {
            return _serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();
        }
        public static ILogger<T> GetLogger<T>()
        {
            return _serviceScope.ServiceProvider.GetService<ILogger<T>>();
        }
        public static UserManager<AppUser> GetUserManager()
        {
            return _serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        }
        public static RoleManager<IdentityRole> GetRoleManager()
        {
            return _serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }
        public static IConfiguration GetConfiguration()
        {
            return _serviceScope.ServiceProvider.GetService<IConfiguration>();
        }
     
        public static ITransactionService GetTransactionService()
        {
            return _serviceScope.ServiceProvider.GetService<ITransactionService>();
        }
        public static IUnitOfWork GetUnitOfWork()
        {
            return _serviceScope.ServiceProvider.GetService<IUnitOfWork>();
        }
        //public static IMapper GetMapper()
        //{
        //    var mappingConfig = new MapperConfiguration(mc =>
        //    {
        //        mc.AddProfile(new MappingProfile());
        //    });

        //    return mappingConfig.CreateMapper();
        //}
    }
  }
