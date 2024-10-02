namespace Events.Application.Interfaces.Infrastructure;

public interface IDbInitializerService
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
