using Events.Application.Resources;
using FluentValidation;
using SmartFormat;

namespace Events.Application.Validation.Common;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, int> GreaterThanOrEqualToWithMessage<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int valueToCompare,
        string propertyName
    )
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(valueToCompare)
            .WithMessage(
                Smart.Format(
                    ValidationMessages.GreaterThanOrEqualToRequired,
                    new { propertyName, valueToCompare }
                )
            );
    }

    public static IRuleBuilderOptions<T, int?> GreaterThanOrEqualToWithMessage<T>(
        this IRuleBuilder<T, int?> ruleBuilder,
        int valueToCompare,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(propertyValue => propertyValue is not null && propertyValue >= valueToCompare)
            .WithMessage(
                Smart.Format(
                    ValidationMessages.GreaterThanOrEqualToRequired,
                    new { propertyName, valueToCompare }
                )
            );
    }

    public static IRuleBuilderOptions<T, int?> LessThanOrEqualToWithMessage<T>(
        this IRuleBuilder<T, int?> ruleBuilder,
        int valueToCompare,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(propertyValue => propertyValue is not null && propertyValue <= valueToCompare)
            .WithMessage(
                Smart.Format(
                    ValidationMessages.LessThanOrEqualToRequired,
                    new { propertyName, valueToCompare }
                )
            );
    }

    public static IRuleBuilderOptions<T, string?> NotEmptyWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(Smart.Format(ValidationMessages.PropertyRequired, new { propertyName }));
    }

    public static IRuleBuilderOptions<T, string?> MinimumLengthWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int minimumLength,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(propertyValue =>
                propertyValue is not null && propertyValue.Length >= minimumLength
            )
            .WithMessage(
                Smart.Format(
                    ValidationMessages.MinLengthRequired,
                    new { propertyName, minimumLength }
                )
            );
    }

    public static IRuleBuilderOptions<T, string?> MaximumLengthWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int maximumLength,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(propertyValue =>
                propertyValue is not null && propertyValue.Length <= maximumLength
            )
            .WithMessage(
                Smart.Format(
                    ValidationMessages.MaxLengthExceeded,
                    new { propertyName, maximumLength }
                )
            );
    }

    public static IRuleBuilderOptions<T, string?> MatchesWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        string format,
        string propertyName
    )
    {
        return ruleBuilder
            .NotNull()
            .Matches(format)
            .WithMessage(
                Smart.Format(ValidationMessages.InvalidFormat, new { propertyName, format })
            );
    }

    public static IRuleBuilderOptions<T, string?> EmailAddressWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .NotNull()
            .EmailAddress()
            .WithMessage(
                Smart.Format(
                    ValidationMessages.InvalidFormat,
                    new { propertyName, format = "email" }
                )
            );
    }

    public static IRuleBuilderOptions<T, TProperty?> NotDefaultWithMessage<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(x => x is not null && !x.Equals(default))
            .WithMessage(Smart.Format(ValidationMessages.PropertyRequired, new { propertyName }));
    }

    public static IRuleBuilderOptions<T, DateTime> MustBePresentOrFutureDateWithMessage<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(date => date != default && date >= DateTime.UtcNow)
            .WithMessage(
                Smart.Format(ValidationMessages.PresentOrFutureDateRequired, new { propertyName })
            );
    }

    public static IRuleBuilderOptions<T, DateTime?> MustBePresentOrFutureDateWithMessage<T>(
        this IRuleBuilder<T, DateTime?> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(parameterValue =>
                parameterValue is DateTime date && date != default && date >= DateTime.UtcNow
            )
            .WithMessage(
                Smart.Format(ValidationMessages.PresentOrFutureDateRequired, new { propertyName })
            );
    }

    public static IRuleBuilderOptions<T, DateOnly> MustBePastDateWithMessage<T>(
        this IRuleBuilder<T, DateOnly> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(date => date != default && date < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(
                Smart.Format(ValidationMessages.NotInFutureRequired, new { propertyName })
            );
    }

    public static IRuleBuilderOptions<T, DateOnly?> MustBePastDateWithMessage<T>(
        this IRuleBuilder<T, DateOnly?> ruleBuilder,
        string propertyName
    )
    {
        return ruleBuilder
            .Must(parameterValue =>
                parameterValue is DateOnly date
                && date != default
                && date < DateOnly.FromDateTime(DateTime.UtcNow)
            )
            .WithMessage(
                Smart.Format(ValidationMessages.NotInFutureRequired, new { propertyName })
            );
    }
}
