using Events.Application.Configuration.Settings;
using Events.Application.Models.Notification;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Notification;

public class NotificationSendRequestModelValidator : AbstractValidator<NotificationSendRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public NotificationSendRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Message)
            .NotEmptyWithMessage(nameof(NotificationSendRequestModel.Message))
            .MaximumLengthWithMessage(
                _validationSettings.MaxNotificationMessageLength,
                nameof(NotificationSendRequestModel.Message)
            );
    }
}
