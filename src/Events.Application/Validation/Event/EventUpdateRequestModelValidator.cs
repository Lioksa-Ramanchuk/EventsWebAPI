using Events.Application.Configuration.Settings;
using Events.Application.Models.Event;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Event;

public class EventUpdateRequestModelValidator : AbstractValidator<EventUpdateRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public EventUpdateRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Title)
            .NotEmptyWithMessage(nameof(EventUpdateRequestModel.Title))
            .MaximumLengthWithMessage(
                _validationSettings.EventTitleMaxLength,
                nameof(EventUpdateRequestModel.Title)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.Description)
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(EventUpdateRequestModel.Description)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.EventDate)
            .MustBePresentOrFutureDateWithMessage(nameof(EventUpdateRequestModel.EventDate))
            .When(x => x.EventDate is not null && x.EventDate != default);

        RuleFor(x => x.Location)
            .MaximumLengthWithMessage(
                _validationSettings.EventLocationMaxLength,
                nameof(EventUpdateRequestModel.Location)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.Category)
            .MaximumLengthWithMessage(
                _validationSettings.EventCategoryMaxLength,
                nameof(EventUpdateRequestModel.Category)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Category));

        RuleFor(x => x.MaxParticipantsCount)
            .GreaterThanOrEqualToWithMessage(
                _validationSettings.MinEventMaxParticipantsCount,
                nameof(EventUpdateRequestModel.MaxParticipantsCount)
            )
            .When(x => x.MaxParticipantsCount is not null);
    }
}
