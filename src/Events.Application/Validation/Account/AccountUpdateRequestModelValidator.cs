using Events.Application.Configuration.Settings;
using Events.Application.Models.Account;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Account;

public class AccountUpdateRequestModelValidator : AbstractValidator<AccountUpdateRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public AccountUpdateRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Username)
            .NotEmptyWithMessage(nameof(AccountUpdateRequestModel.Username))
            .MaximumLengthWithMessage(
                _validationSettings.AccountUsernameMaxLength,
                nameof(AccountUpdateRequestModel.Username)
            )
            .MatchesWithMessage(
                _validationSettings.AccountUsernameFormat,
                nameof(AccountUpdateRequestModel.Username)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Username));

        RuleFor(x => x.Password)
            .NotEmptyWithMessage(nameof(AccountUpdateRequestModel.Password))
            .MinimumLengthWithMessage(
                _validationSettings.AccountPasswordMinLength,
                nameof(AccountUpdateRequestModel.Password)
            )
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(AccountUpdateRequestModel.Password)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}
