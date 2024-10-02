namespace Events.Application.Interfaces.Infrastructure;

public interface ICryptoService
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}
