using Events.Application.Configuration.Settings;
using Events.Application.Models.Account;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Account;

public class AccountSignInRequestModelValidator : AbstractValidator<AccountSignInRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public AccountSignInRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Username)
            .NotEmptyWithMessage(nameof(AccountSignInRequestModel.Username))
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(AccountSignInRequestModel.Username)
            );
        RuleFor(x => x.Password)
            .NotEmptyWithMessage(nameof(AccountSignInRequestModel.Password))
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(AccountSignInRequestModel.Password)
            );
    }
}
