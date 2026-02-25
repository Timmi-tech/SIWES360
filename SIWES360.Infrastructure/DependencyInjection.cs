using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SIWES360.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using SIWES360.Application.Common.Interfaces;

namespace SIWES360.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgresConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            return services;
        }
    }
}
