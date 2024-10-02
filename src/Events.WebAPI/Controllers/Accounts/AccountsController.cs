using System.Net.Mime;
using System.Security.Claims;
using Events.Application.Interfaces.Infrastructure;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Account;
using Events.Application.Models.Common;
using Events.Application.Models.Participant;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions;
using Events.WebAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Accounts;

[Route("api/accounts")]
[ApiController]
public class AccountsController(
    IAccountService accountService,
    IJwtManagementHelperService JwtManagementHelperService
) : ControllerBase
{
    /// <summary>
    /// Registers a new account.
    /// </summary>
    /// <param name="signUpModel">The information for the account to register.</param>
    /// <returns>The created account information.</returns>
    [HttpPost("sign-up")]
    [SwaggerOperation("SignUp")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpAsParticipantAsync(
        [FromBody] ParticipantSignUpRequestModel signUpModel,
        CancellationToken ct
    )
    {
        var participantModel = await accountService.RegisterParticipantAsync(signUpModel, ct);
        return CreatedAtAction(
            nameof(GetAccountByIdAsync),
            new { accountId = participantModel.Id },
            participantModel
        );
    }

    /// <summary>
    /// Authenticates an account and issues tokens.
    /// </summary>
    /// <param name="signInModel">The account's sign-in credentials.</param>
    /// <returns>Success message upon successful sign-in.</returns>
    [HttpPost("sign-in")]
    [SwaggerOperation("SignIn")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignInAsync(
        [FromBody] AccountSignInRequestModel signInModel,
        CancellationToken ct
    )
    {
        Account accountWithRoles = await accountService.AuthenticateAsync(signInModel, ct);
        await JwtManagementHelperService.SetTokensAsync(accountWithRoles, ct);
        return Ok(new { message = ResponseMessages.SignInSuccess });
    }

    /// <summary>
    /// Signs out the current account.
    /// </summary>
    /// <returns>Success message upon successful sign-out.</returns>
    [HttpPost("sign-out")]
    [Authorize]
    [SwaggerOperation("SignOut")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    public new IActionResult SignOut()
    {
        JwtManagementHelperService.ClearTokens();
        return Ok(new { message = ResponseMessages.SignOutSuccess });
    }

    /// <summary>
    /// Refreshes the JWT tokens for the current account.
    /// </summary>
    /// <returns>Success message upon successful token refresh.</returns>
    [HttpPost("refresh-tokens")]
    [SwaggerOperation("RefreshTokens")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshTokensAsync(CancellationToken ct)
    {
        await JwtManagementHelperService.RefreshTokensAsync(ct);
        return Ok(new { message = ResponseMessages.TokensRefreshSuccess });
    }

    /// <summary>
    /// Retrieves the current account's information.
    /// </summary>
    /// <returns>The current account's information.</returns>
    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation("GetCurrentAccount")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccountResponseModel>> GetCurrentAccount(CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new ForbiddenException();
        }

        return Ok(await accountService.GetAccountByIdAsModelAsync(userId, ct));
    }

    /// <summary>
    /// Retrieves account information by ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to retrieve.</param>
    /// <returns>The account information associated with the specified ID.</returns>
    [HttpGet($"{{{RouteKeys.AccountId}:guid}}")]
    [SwaggerOperation("GetAccountById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountByIdAsync(
        [FromRoute] Guid accountId,
        CancellationToken ct
    )
    {
        return Ok(await accountService.GetAccountByIdAsModelAsync(accountId, ct));
    }

    /// <summary>
    /// Retrieves all accounts, optionally filtered.
    /// </summary>
    /// <param name="filterModel">The filter criteria for retrieving accounts.</param>
    /// <returns>A list of accounts matching the filter criteria.</returns>
    [HttpGet]
    [SwaggerOperation("GetAllAccounts")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<AccountResponseModel>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAllAccountsAsync(
        [FromQuery] AccountFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        return Ok(await accountService.GetAllAccountsAsModelsAsync(filterModel, ct));
    }

    /// <summary>
    /// Updates the details of a specified account.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="updateModel">The updated account details.</param>
    /// <returns>The updated account information.</returns>
    [HttpPatch($"{{{RouteKeys.AccountId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AccountOwnerOrAdministratorPolicy)]
    [SwaggerOperation("UpdateAccount")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AccountResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAccountAsync(
        [FromRoute] Guid accountId,
        [FromBody] AccountUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        return Ok(await accountService.UpdateAccountAsync(accountId, updateModel, ct));
    }

    /// <summary>
    /// Removes an account by ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to remove.</param>
    /// <returns>No content.</returns>
    [HttpDelete($"{{{RouteKeys.AccountId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AccountOwnerOrAdministratorPolicy)]
    [SwaggerOperation("RemoveAccount")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAccountAsync(
        [FromRoute] Guid accountId,
        CancellationToken ct
    )
    {
        await accountService.RemoveAccountAsync(accountId, ct);
        return NoContent();
    }
}
