using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIWES360.Application.Common.Interfaces;
using SIWES360.Domain.Entities.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SIWES360.Infrastructure.Persistence;
using SIWES360.Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<JwtConfiguration>(configuration.GetSection(JwtConfiguration.Section));

            var jwt = configuration.GetSection(JwtConfiguration.Section).Get<JwtConfiguration>()
                  ?? throw new InvalidOperationException("JwtSettings missing");

            if (string.IsNullOrWhiteSpace(jwt.Secret))
                throw new InvalidOperationException("JwtSettings:Secret is missing");

            if (string.IsNullOrWhiteSpace(jwt.Secret))
                throw new InvalidOperationException("JwtSettings:Secret is missing");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.ValidIssuer,
                    ValidAudience = jwt.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}
