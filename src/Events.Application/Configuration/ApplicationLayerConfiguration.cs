using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Account;
using Events.Application.Models.AccountRole.AccountRole;
using Events.Application.Models.Event;
using Events.Application.Models.EventParticipant.EventParticipant;
using Events.Application.Models.Notification;
using Events.Application.Models.Participant;
using Events.Application.Services;
using Events.Application.Services.Hosted;
using Events.Application.Validation.Account;
using Events.Application.Validation.AccountRole;
using Events.Application.Validation.Event;
using Events.Application.Validation.EventParticipant;
using Events.Application.Validation.Notification;
using Events.Application.Validation.Participant;
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
        services.AddFluentValidationAutoValidation();
        services.AddScoped<
            IValidator<AccountFilterRequestModel>,
            AccountFilterRequestModelValidator
        >();
        services.AddScoped<
            IValidator<AccountRoleFilterRequestModel>,
            AccountRoleFilterRequestModelValidator
        >();
        services.AddScoped<
            IValidator<AccountSignInRequestModel>,
            AccountSignInRequestModelValidator
        >();
        services.AddScoped<
            IValidator<AccountUpdateRequestModel>,
            AccountUpdateRequestModelValidator
        >();
        services.AddScoped<IValidator<EventAddRequestModel>, EventAddRequestModelValidator>();
        services.AddScoped<IValidator<EventFilterRequestModel>, EventFilterRequestModelValidator>();
        services.AddScoped<
            IValidator<EventParticipantFilterRequestModel>,
            EventParticipantFilterRequestModelValidator
        >();
        services.AddScoped<IValidator<EventSearchRequestModel>, EventSearchRequestModelValidator>();
        services.AddScoped<IValidator<EventUpdateRequestModel>, EventUpdateRequestModelValidator>();
        services.AddScoped<
            IValidator<ParticipantSignUpRequestModel>,
            ParticipantSignUpRequestModelValidator
        >();
        services.AddScoped<
            IValidator<ParticipantUpdateRequestModel>,
            ParticipantUpdateRequestModelValidator
        >();
        services.AddScoped<
            IValidator<NotificationSendRequestModel>,
            NotificationSendRequestModelValidator
        >();

        return services;
    }
}
