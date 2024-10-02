using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class RoleRepository(ApplicationDbContext dbContext, DeferredCommandManager commandManager)
    : BaseRepository(dbContext, commandManager),
        IRoleRepository
{
    public void Add(Role role)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));

        base.Add(role);
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<Role?> GetByTitleAsync(string title, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));

        return await _dbContext
            .Roles.AsNoTracking()
            .FirstOrDefaultAsync(
                r => EF.Functions.Collate(r.Title, CaseInsensitiveCollation) == title,
                ct
            );
    }
}
