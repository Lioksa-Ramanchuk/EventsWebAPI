using Events.Application.Configuration.Settings;
using Events.Application.Models.Event;
using Events.Application.Resources;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Application.Validation.Event;

public class EventFilterRequestModelValidator : AbstractValidator<EventFilterRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public EventFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        Include(new BaseFilterRequestModelValidator<EventFilterRequestModel>(appSettings));

        RuleFor(x => x)
            .Must(x => x.StartEventDate!.Value <= x.EndEventDate!.Value)
            .WithMessage(
                Smart.Format(
                    ValidationMessages.InvalidDateRange,
                    new
                    {
                        startDatePropertyName = nameof(EventFilterRequestModel.StartEventDate),
                        endDatePropertyName = nameof(EventFilterRequestModel.EndEventDate),
                    }
                )
            )
            .When(x =>
                x.StartEventDate is not null
                && x.StartEventDate.Value != default
                && x.EndEventDate is not null
                && x.EndEventDate.Value != default
            );

        RuleFor(x => x.Location)
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(EventUpdateRequestModel.Location)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.Category)
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(EventUpdateRequestModel.Category)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Category));
    }
}
