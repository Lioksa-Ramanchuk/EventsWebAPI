using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public interface IDeferredCommand
{
    Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken ct = default);
}
