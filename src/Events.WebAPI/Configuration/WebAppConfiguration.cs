using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Constants;
using Events.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Events.WebAPI.Configuration;

public static class WebAppConfiguration
{
    public static async Task<WebApplication> ConfigurePipelineAsync(
        this WebApplication app,
        AppSettings appSettings,
        CancellationToken cancellationToken
    )
    {
        using (var scope = app.Services.CreateScope())
        {
            await scope
                .ServiceProvider.GetRequiredService<IDbInitializerService>()
                .InitializeAsync(cancellationToken);
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseStaticFiles(
            new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                    ctx.Context.Response.Headers.CacheControl =
                        $"public, max-age={appSettings.CacheSettings.DurationInSeconds}",
            }
        );

        app.UseCors(CorsPolicies.AllowSpecificOrigins);

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events Web API"));

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
