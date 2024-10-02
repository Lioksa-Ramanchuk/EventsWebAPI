using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Services;

public class CryptoService(IOptions<AppSettings> appSettings) : ICryptoService
{
    private readonly CryptoSettings _cryptoSettings = appSettings.Value.CryptoSettings;

    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, _cryptoSettings.WorkFactor);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword, nameof(hashedPassword));
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
