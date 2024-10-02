using Events.Infrastructure.Data.Context;
using Events.Tests.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartFormat;

namespace Events.Tests.Common.Fixtures;

public class DbFixture : IAsyncLifetime
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly string _connectionString;
    public ApplicationDbContext DbContext { get; private set; } = null!;

    public DbFixture()
    {
        var appSettings = ConfigurationHelper.LoadAppSettings(
            ConfigurationHelper.TestAppSettingsFileName
        );
        _connectionString = GenerateTestConnectionString(
            appSettings.ConnectionStrings.TestConnectionFormat
        );
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;
    }

    public async Task InitializeAsync()
    {
        DbContext = new ApplicationDbContext(_options);
        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
    }

    private static string GenerateTestConnectionString(string connectionStringFormat)
    {
        var formatter = Smart.CreateDefaultSmartFormat();
        formatter.Settings.Parser.ConvertCharacterStringLiterals = false;

        return formatter.Format(
            connectionStringFormat,
            new { database = Guid.NewGuid().ToString() }
        );
    }
}
