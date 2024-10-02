using Events.Application.Configuration.Settings;
using Events.Application.Models.Account;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Account;

public class AccountFilterRequestModelValidator : AbstractValidator<AccountFilterRequestModel>
{
    public AccountFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        Include(new BaseFilterRequestModelValidator<AccountFilterRequestModel>(appSettings));
    }
}
