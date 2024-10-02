using Events.Application.Configuration.Settings;
using Events.Application.Models.Event;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Event;

public class EventAddRequestModelValidator : AbstractValidator<EventAddRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public EventAddRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Title)
            .NotEmptyWithMessage(nameof(EventAddRequestModel.Title))
            .MaximumLengthWithMessage(
                _validationSettings.EventTitleMaxLength,
                nameof(EventAddRequestModel.Title)
            );

        RuleFor(x => x.Description)
            .NotEmptyWithMessage(nameof(EventAddRequestModel.Description))
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(EventAddRequestModel.Description)
            );

        RuleFor(x => x.EventDate)
            .NotDefaultWithMessage(nameof(EventAddRequestModel.EventDate))
            .MustBePresentOrFutureDateWithMessage(nameof(EventAddRequestModel.EventDate));

        RuleFor(x => x.Location)
            .NotEmptyWithMessage(nameof(EventAddRequestModel.Location))
            .MaximumLengthWithMessage(
                _validationSettings.EventLocationMaxLength,
                nameof(EventAddRequestModel.Location)
            );

        RuleFor(x => x.Category)
            .NotEmptyWithMessage(nameof(EventAddRequestModel.Category))
            .MaximumLengthWithMessage(
                _validationSettings.EventCategoryMaxLength,
                nameof(EventAddRequestModel.Category)
            );

        RuleFor(x => x.MaxParticipantsCount)
            .GreaterThanOrEqualToWithMessage(
                _validationSettings.MinEventMaxParticipantsCount,
                nameof(EventAddRequestModel.MaxParticipantsCount)
            );
    }
}
