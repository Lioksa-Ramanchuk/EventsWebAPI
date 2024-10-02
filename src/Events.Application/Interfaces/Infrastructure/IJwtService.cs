using System.Security.Claims;
using Events.Domain.Entities;

namespace Events.Application.Interfaces.Infrastructure;

public interface IJwtService
{
    string GenerateAccessToken(Account accountWithRoles);
    string GenerateRefreshToken();
    Guid GetAccountIdFromToken(string token);
    ClaimsPrincipal GetPrincipalFromToken(string token);
}
