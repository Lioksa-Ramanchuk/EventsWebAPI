using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Constants;
using Events.Domain.Exceptions.AuthExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Services;

public class RoleManagementHelperService(
    IDbUnitOfWork db,
    IAuthorizationHelperService authorizationHelperService,
    IOptions<AppSettings> appSettings
) : IRoleManagementHelperService
{
    private readonly AuthorizationSettings _authorizationSettings = appSettings
        .Value
        .AuthorizationSettings;

    public string AdministratorRoleTitle => _authorizationSettings.RoleTitles.Administrator;
    public string ParticipantRoleTitle => _authorizationSettings.RoleTitles.Participant;

    public async Task<bool> IsLastAdministratorByIdAsync(Guid accountId, CancellationToken ct)
    {
        var accountWithRoles =
            await db.Accounts.GetByIdWithRolesAsync(accountId, ct)
            ?? throw new AccountNotFoundException(accountId);

        if (
            !await authorizationHelperService.PassesPolicyAsync(
                accountWithRoles,
                AuthPolicies.AdministratorPolicy,
                ct
            )
        )
        {
            return false;
        }

        return !await db.AccountRoles.AnyOtherAccountWithRoleTitleAsync(
            accountId,
            _authorizationSettings.RoleTitles.Administrator,
            ct
        );
    }

    public async Task<List<string>> GetInitialParticipantRoleTitlesAsync(CancellationToken ct)
    {
        var roles = new List<string> { _authorizationSettings.RoleTitles.Participant };

        if (
            !await db.AccountRoles.AnyAccountWithRoleByTitle(
                _authorizationSettings.RoleTitles.Administrator,
                ct
            )
        )
        {
            roles.Add(_authorizationSettings.RoleTitles.Administrator);
        }

        return roles;
    }
}
