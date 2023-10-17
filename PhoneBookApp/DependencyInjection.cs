using PhoneBookApp.Common.Interfaces;
using PhoneBookApp.Services;
using System.Reflection;

namespace PhoneBookApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IDbInitializer, DbInitializer>();

            return services;
        }
    }
}
