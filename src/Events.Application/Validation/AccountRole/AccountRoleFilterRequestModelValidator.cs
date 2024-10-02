using Events.Application.Configuration.Settings;
using Events.Application.Models.AccountRole.AccountRole;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.AccountRole;

public class AccountRoleFilterRequestModelValidator
    : AbstractValidator<AccountRoleFilterRequestModel>
{
    public AccountRoleFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        Include(new BaseFilterRequestModelValidator<AccountRoleFilterRequestModel>(appSettings));
    }
}
