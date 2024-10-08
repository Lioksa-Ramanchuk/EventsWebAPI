using Events.Application.Configuration;
using Events.Application.Configuration.Settings;
using Events.Infrastructure.Configuration;
using Events.WebAPI.Configuration;
using Serilog;

namespace Events;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        if (!Directory.Exists(wwwrootPath))
        {
            Directory.CreateDirectory(wwwrootPath);
        }

        var builder = WebApplication.CreateBuilder(args);

        var appSettingsConfig = builder.Configuration.GetRequiredSection(nameof(AppSettings));
        builder.Services.Configure<AppSettings>(appSettingsConfig);

        var appSettings = appSettingsConfig.Get<AppSettings>()!;

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        Log.Logger = logger;

        try
        {
            builder.Host.UseSerilog(logger);

            Log.Information("Adding infrastructure layer services...");
            builder.Services.AddInfrastructureServices(appSettings);

            Log.Information("Adding application layer services...");
            builder.Services.AddApplicationServices(appSettings);

            Log.Information("Adding web API layer services...");
            builder.Services.AddWebServices(appSettings);

            Log.Information("Building the web application...");
            var app = builder.Build();

            Log.Information("Configuring web application pipeline...");
            await app.ConfigurePipelineAsync(appSettings, cancellationToken);

            Log.Information("Server is running! (https://localhost:5001/swagger)");
            await app.RunAsync(cancellationToken);
        }
        finally
        {
            Log.Information("Shutting down...");
            Log.CloseAndFlush();
        }
    }
}
