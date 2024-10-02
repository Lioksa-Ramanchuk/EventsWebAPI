using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.AccountRole.AccountRole;
using Events.Application.Models.Common;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Accounts;

[Route($"api/accounts/{{{RouteKeys.AccountId}:guid}}/roles")]
[ApiController]
public class AccountRolesController(IAccountRoleService accountRolesService) : ControllerBase
{
    /// <summary>
    /// Grants a role to an account.
    /// </summary>
    /// <param name="accountId">The ID of the account to which the role will be granted.</param>
    /// <param name="roleId">The ID of the role to grant.</param>
    /// <returns>The granted account role information.</returns>
    [HttpPost($"{{{RouteKeys.RoleId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("GrantAccountRole")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountRoleResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GrantAccountRoleAsync(
        [FromRoute] Guid accountId,
        [FromRoute] Guid roleId,
        CancellationToken ct
    )
    {
        var accountRoleModel = await accountRolesService.GrantAccountRoleAsync(
            accountId,
            roleId,
            ct
        );
        return CreatedAtAction(
            nameof(GetAccountRoleByKeyAsync),
            new { accountId, roleId },
            accountRoleModel
        );
    }

    /// <summary>
    /// Retrieves a specific role assigned to an account.
    /// </summary>
    /// <param name="accountId">The ID of the account.</param>
    /// <param name="roleId">The ID of the role to retrieve.</param>
    /// <returns>The role information assigned to the specified account.</returns>
    [HttpGet($"{{{RouteKeys.RoleId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AccountOwnerOrAdministratorPolicy)]
    [SwaggerOperation("GetAccountRoleByKey")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountRoleResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountRoleByKeyAsync(
        [FromRoute] Guid accountId,
        [FromRoute] Guid roleId,
        CancellationToken ct
    )
    {
        return Ok(await accountRolesService.GetAccountRoleByKeyAsModelAsync(accountId, roleId, ct));
    }

    /// <summary>
    /// Retrieves all roles assigned to a specific account.
    /// </summary>
    /// <param name="accountId">The ID of the account whose roles to retrieve.</param>
    /// <param name="filterModel">The filter criteria for retrieving account roles.</param>
    /// <returns>A list of roles assigned to the specified account.</returns>
    [HttpGet]
    [Authorize(Policy = AuthPolicies.AccountOwnerOrAdministratorPolicy)]
    [SwaggerOperation("GetAccountRoles")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<AccountRoleResponseModel>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountRolesAsync(
        [FromRoute] Guid accountId,
        [FromQuery] AccountRoleFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        return Ok(
            await accountRolesService.GetAccountRolesAsModelsAsync(accountId, filterModel, ct)
        );
    }

    /// <summary>
    /// Revokes a role from an account.
    /// </summary>
    /// <param name="accountId">The ID of the account from which the role will be revoked.</param>
    /// <param name="roleId">The ID of the role to revoke.</param>
    /// <returns>No content upon successful revocation.</returns>
    [HttpDelete($"{{{RouteKeys.RoleId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("RevokeAccountRole")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeAccountRoleAsync(
        [FromRoute] Guid accountId,
        [FromRoute] Guid roleId,
        CancellationToken ct
    )
    {
        await accountRolesService.RevokeAccountRoleAsync(accountId, roleId, ct);
        return NoContent();
    }
}
