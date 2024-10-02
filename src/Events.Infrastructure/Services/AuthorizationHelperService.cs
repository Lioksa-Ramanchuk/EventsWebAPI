using System.Security.Claims;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Events.Infrastructure.Services;

public class AuthorizationHelperService(IAuthorizationService authorizationService)
    : IAuthorizationHelperService
{
    public async Task<bool> PassesPolicyAsync(
        Account accountWithRoles,
        string policy,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(accountWithRoles, nameof(accountWithRoles));
        ArgumentException.ThrowIfNullOrEmpty(policy, nameof(policy));

        var claims = GetAccountClaims(accountWithRoles);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return (await authorizationService.AuthorizeAsync(principal, policy)).Succeeded;
    }

    public List<Claim> GetAccountClaims(Account accountWithRoles)
    {
        ArgumentNullException.ThrowIfNull(accountWithRoles, nameof(accountWithRoles));

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, accountWithRoles.Username),
            new(ClaimTypes.NameIdentifier, accountWithRoles.Id.ToString()),
        };
        claims.AddRange(
            accountWithRoles.AccountRoles.Select(ar => new Claim(ClaimTypes.Role, ar.Role.Title))
        );

        return claims;
    }
}
