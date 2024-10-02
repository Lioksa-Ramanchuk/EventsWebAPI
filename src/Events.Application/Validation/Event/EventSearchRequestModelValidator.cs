using Events.Application.Configuration.Settings;
using Events.Application.Models.Event;
using Events.Application.Resources;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Event;

public class EventSearchRequestModelValidator : AbstractValidator<EventSearchRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public EventSearchRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x)
            .Must(x =>
            {
                return !string.IsNullOrWhiteSpace(x.Title);
            })
            .WithMessage(ValidationMessages.EventSearchCriteriaRequired);

        RuleFor(x => x.Title)
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(EventSearchRequestModel.Title)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Title));
    }
}
