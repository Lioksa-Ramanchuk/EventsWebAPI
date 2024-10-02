using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Infrastructure.Data.UnitOfWork;
using Events.Infrastructure.Services;
using Events.Infrastructure.Services.Hosted;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Tests.Common.Configuration;

public static class TestInfrastructureServiceExtensions
{
    public static IServiceCollection AddTestInfrastructureServices(
        this IServiceCollection services,
        DbUnitOfWork dbUnitOfWork,
        AppSettings _
    )
    {
        services.AddSingleton<IWebHostEnvironment>(new TestWebHostEnvironment());
        services.AddHttpContextAccessor();
        services.AddScoped<IDbInitializerService, DbInitializerService>();
        services.AddScoped<IDbUnitOfWork>(provider => dbUnitOfWork);
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
}
