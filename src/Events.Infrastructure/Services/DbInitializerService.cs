using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Services;

public class DbInitializerService(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
    : IDbInitializerService
{
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
        await SeedRolesAsync(cancellationToken);
    }

    private async Task SeedRolesAsync(CancellationToken cancellationToken)
    {
        if (!await dbContext.Roles.AnyAsync(cancellationToken))
        {
            await dbContext.Roles.AddRangeAsync(
                [
                    new() { Title = _appSettings.AuthorizationSettings.RoleTitles.Administrator },
                    new() { Title = _appSettings.AuthorizationSettings.RoleTitles.Participant },
                ],
                cancellationToken
            );
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
