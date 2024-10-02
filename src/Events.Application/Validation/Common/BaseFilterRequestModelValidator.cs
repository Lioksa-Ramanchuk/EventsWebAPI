using Events.Application.Configuration.Settings;
using Events.Application.Models.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Common;

public class BaseFilterRequestModelValidator<T> : AbstractValidator<T>
    where T : BaseFilterRequestModel
{
    private readonly ValidationSettings _validationSettings;

    public BaseFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Offset)
            .GreaterThanOrEqualToWithMessage(0, nameof(BaseFilterRequestModel.Offset))
            .When(x => x.Offset is not null);

        RuleFor(x => x.Limit)
            .GreaterThanOrEqualToWithMessage(1, nameof(BaseFilterRequestModel.Limit))
            .LessThanOrEqualToWithMessage(
                _validationSettings.MaxFilterLimit,
                nameof(BaseFilterRequestModel.Limit)
            )
            .When(x => x.Limit is not null);
    }
}
