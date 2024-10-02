using Events.Infrastructure.Services;
using Events.Tests.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Events.Tests.UnitTests.Infrastructure.Services;

public class CryptoServiceTests
{
    private readonly CryptoService _cryptoService;

    public CryptoServiceTests()
    {
        var appSettings = ConfigurationHelper.LoadAppSettings(
            ConfigurationHelper.TestAppSettingsFileName
        );

        _cryptoService = new CryptoService(Options.Create(appSettings));
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "password";

        // Act
        var hashedPassword = _cryptoService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.False(string.IsNullOrEmpty(hashedPassword));
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
    {
        // Arrange
        var password = "password";
        var hashedPassword = _cryptoService.HashPassword(password);

        // Act
        var isValid = _cryptoService.VerifyPassword(hashedPassword, password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var password = "password";
        var hashedPassword = _cryptoService.HashPassword(password);

        // Act
        var isValid = _cryptoService.VerifyPassword(hashedPassword, "wrongPassword");

        // Assert
        Assert.False(isValid);
    }
}
