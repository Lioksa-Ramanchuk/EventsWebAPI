using Events.Application.Configuration.Settings;
using Microsoft.Extensions.Configuration;

namespace Events.Tests.Common.Configuration;

public static class ConfigurationHelper
{
    public const string TestAppSettingsFileName = "appsettings.Test.json";

    public static AppSettings LoadAppSettings(string jsonFileName)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(jsonFileName)
            .Build();

        var appSettings = new AppSettings();
        configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
        return appSettings;
    }
}
