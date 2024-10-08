using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Services;
using Events.Application.Services.Hosted;
using Events.Application.Validation.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application.Configuration;

public static class ApplicationLayerConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        AppSettings appSettings
    )
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountRoleService, AccountRoleService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IParticipantService, ParticipantService>();
        services.AddScoped<IEventParticipantService, EventParticipantService>();
        services.AddScoped<IParticipantEventService, ParticipantEventService>();

        services.AddHostedService<EventParticipantsNotificationService>();

        services.ConfigureValidation(appSettings);

        services.AddAutoMapper(typeof(ApplicationLayerConfiguration).Assembly);

        return services;
    }

    private static IServiceCollection ConfigureValidation(
        this IServiceCollection services,
        AppSettings _
    )
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ValidatorExtensions));
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        return services;
    }
}
