using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions.JwtExceptions;
using Events.Domain.Resources;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Events.Infrastructure.Services;

public class JwtService(
    IOptions<AppSettings> appSettings,
    IAuthorizationHelperService authorizationHelperService
) : IJwtService
{
    private readonly JwtSettings _jwtSettings = appSettings.Value.JwtSettings;

    public string GenerateAccessToken(Account accountWithRoles)
    {
        ArgumentNullException.ThrowIfNull(accountWithRoles, nameof(accountWithRoles));

        var claims = authorizationHelperService.GetAccountClaims(accountWithRoles);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddSeconds(_jwtSettings.AccessTokenExpiresInSeconds);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audiences.FirstOrDefault(),
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public Guid GetAccountIdFromToken(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        var principal = GetPrincipalFromToken(token);
        var accountIdClaim =
            principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidTokenException(ExceptionMessages.InvalidTokenIdClaimMissing);

        if (!Guid.TryParse(accountIdClaim, out Guid accountId))
        {
            throw new InvalidTokenException(ExceptionMessages.InvalidTokenIdClaimInvalid);
        }

        return accountId;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudiences = _jwtSettings.Audiences,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            ),

            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
        catch (SecurityTokenException ex)
        {
            throw new InvalidTokenException(ExceptionMessages.InvalidToken, ex);
        }
    }
}
