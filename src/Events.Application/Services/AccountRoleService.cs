using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Application.Interfaces.Services;
using Events.Application.Models.AccountRole.AccountRole;
using Events.Application.Models.Common;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Application.Services;

public class AccountRoleService(
    IDbUnitOfWork db,
    IMapper mapper,
    IOptions<AppSettings> appSettings,
    IRoleManagementHelperService roleManagementHelperService
) : IAccountRoleService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<AccountRoleResponseModel> GrantAccountRoleAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct
    )
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        if (await db.Roles.GetByIdAsync(roleId, ct) is null)
        {
            throw new RoleNotFoundException(roleId);
        }

        if (await db.AccountRoles.GetByKeyAsync(accountId, roleId, ct) is not null)
        {
            throw new AccountAlreadyHasRoleException(accountId, roleId);
        }

        var accountRole = new AccountRole
        {
            AccountId = accountId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow,
        };

        db.AccountRoles.Add(accountRole);
        await db.SaveChangesAsync(ct);

        return mapper.Map<AccountRoleResponseModel>(accountRole);
    }

    public async Task<AccountRoleResponseModel> GetAccountRoleByKeyAsModelAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct
    )
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        if (await db.Roles.GetByIdAsync(roleId, ct) is null)
        {
            throw new RoleNotFoundException(roleId);
        }

        var accountRole =
            await db.AccountRoles.GetByKeyWithRoleAsync(accountId, roleId, ct)
            ?? throw new AccountRoleNotFoundException(accountId, roleId);

        return mapper.Map<AccountRoleResponseModel>(accountRole);
    }

    public async Task<PagedResponseModel<AccountRoleResponseModel>> GetAccountRolesAsModelsAsync(
        Guid accountId,
        AccountRoleFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (accountRolesWithRoleList, totalCount) =
            await db.AccountRoles.GetPagedAllWithRoleByAccountIdAsync(accountId, offset, limit, ct);
        var accountRoleModels = mapper.Map<ICollection<AccountRoleResponseModel>>(
            accountRolesWithRoleList
        );

        return new PagedResponseModel<AccountRoleResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = accountRoleModels,
        };
    }

    public async Task RevokeAccountRoleAsync(Guid accountId, Guid roleId, CancellationToken ct)
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        var role =
            await db.Roles.GetByIdAsync(roleId, ct) ?? throw new RoleNotFoundException(roleId);

        var accountRole =
            await db.AccountRoles.GetByKeyAsTrackingAsync(accountId, roleId, ct)
            ?? throw new AccountRoleNotAssignedException(accountId, roleId);

        if (
            string.Equals(
                role.Title,
                roleManagementHelperService.AdministratorRoleTitle,
                StringComparison.OrdinalIgnoreCase
            ) && await roleManagementHelperService.IsLastAdministratorByIdAsync(accountId, ct)
        )
        {
            throw new LastAdministratorRoleRevocationException(accountId);
        }

        db.AccountRoles.Remove(accountRole);
        await db.SaveChangesAsync(ct);
    }
}
