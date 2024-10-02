using System.Security.Claims;
using Events.Domain.Entities;

namespace Events.Application.Interfaces.Infrastructure;

public interface IAuthorizationHelperService
{
    Task<bool> PassesPolicyAsync(
        Account accountWithRoles,
        string policy,
        CancellationToken ct = default
    );
    List<Claim> GetAccountClaims(Account accountWithRoles);
}
