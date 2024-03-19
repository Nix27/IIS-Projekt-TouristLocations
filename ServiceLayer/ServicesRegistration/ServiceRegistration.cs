using DAL.Context;
using DAL.Repository.Abstraction;
using DAL.Repository.Implementation;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.Service.Implementation;

namespace ServiceLayer.ServicesRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(configuration[connectionString]);
            });
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void RegisterUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
