using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Infrastructure.Data.Context;
using Events.Infrastructure.Data.UnitOfWork;
using Events.Infrastructure.Services;
using Events.Infrastructure.Services.Hosted;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.Configuration;

public static class InfrastructureLayerConfiguration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        services.ConfigureDatabase(appSettings);

        services.AddHttpContextAccessor();
        services.AddScoped<IDbInitializerService, DbInitializerService>();
        services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();
        services.AddScoped<ICryptoService, CryptoService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IAuthorizationHelperService, AuthorizationHelperService>();
        services.AddScoped<IRoleManagementHelperService, RoleManagementHelperService>();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IJwtManagementHelperService, JwtManagementHelperService>();

        services.AddHostedService<ExpiredStoredTokensCleanupService>();

        return services;
    }

    private static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                options.UseSqlServer(
                    appSettings.ConnectionStrings.DefaultConnection,
                    opt => opt.MigrationsAssembly($"{nameof(Events)}.{nameof(Infrastructure)}")
                );
            }
        );

        return services;
    }
}
