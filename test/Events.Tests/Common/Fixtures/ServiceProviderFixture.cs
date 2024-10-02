using Events.Application.Configuration;
using Events.Application.Configuration.Settings;
using Events.Tests.Common.Configuration;
using Events.Tests.Common.Data;
using Events.WebAPI.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Tests.Common.Fixtures;

public class ServiceProviderFixture(DbFixture dbFixture) : DbTestBase(dbFixture)
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        var services = new ServiceCollection();

        var appSettings = ConfigurationHelper.LoadAppSettings(
            ConfigurationHelper.TestAppSettingsFileName
        );
        services.Configure<AppSettings>(options => options = appSettings);

        services.AddTestInfrastructureServices(_dbUnitOfWork, appSettings);
        services.AddApplicationServices(appSettings);
        services.AddWebServices(appSettings);

        ServiceProvider = services.BuildServiceProvider();
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        (ServiceProvider as IDisposable)?.Dispose();
    }
}
