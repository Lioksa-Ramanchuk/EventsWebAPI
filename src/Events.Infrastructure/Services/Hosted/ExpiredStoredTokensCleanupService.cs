using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Services.Hosted;

public class ExpiredStoredTokensCleanupService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        int cleanUpIntervalInSeconds;
        using (var scope = serviceProvider.CreateScope())
        {
            cleanUpIntervalInSeconds = scope
                .ServiceProvider.GetRequiredService<IOptions<AppSettings>>()
                .Value.JwtSettings.ExpiredRefreshTokensCleanupIntervalInSeconds;
        }

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(cleanUpIntervalInSeconds), ct);
            await CleanupExpiredStoredTokens(ct);
        }
    }

    private async Task CleanupExpiredStoredTokens(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var JwtManagementHelperService =
            scope.ServiceProvider.GetRequiredService<IJwtManagementHelperService>();
        await JwtManagementHelperService.CleanupExpiredStoredTokensAsync(ct);
    }
}
