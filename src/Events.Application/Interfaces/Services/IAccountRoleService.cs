using Events.Application.Models.AccountRole.AccountRole;
using Events.Application.Models.Common;

namespace Events.Application.Interfaces.Services;

public interface IAccountRoleService
{
    Task<AccountRoleResponseModel> GrantAccountRoleAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct = default
    );
    Task<AccountRoleResponseModel> GetAccountRoleByKeyAsModelAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct = default
    );
    Task<PagedResponseModel<AccountRoleResponseModel>> GetAccountRolesAsModelsAsync(
        Guid accountId,
        AccountRoleFilterRequestModel filterModel,
        CancellationToken ct = default
    );
    Task RevokeAccountRoleAsync(Guid accountId, Guid roleId, CancellationToken ct = default);
}
