using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class AccountRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), IAccountRepository
{
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Account?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Accounts.FindAsync([id], ct);
    }

    public async Task<Account?> GetByIdWithRolesAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext
            .Accounts.AsNoTracking()
            .Include(a => a.AccountRoles)
            .ThenInclude(ar => ar.Role)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Account?> GetByUsernameWithRolesAsync(string username, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        return await _dbContext
            .Accounts.AsNoTracking()
            .Include(a => a.AccountRoles)
            .ThenInclude(ar => ar.Role)
            .FirstOrDefaultAsync(
                a => EF.Functions.Collate(a.Username, CaseInsensitiveCollation) == username,
                ct
            );
    }

    public async Task<(List<Account> accounts, int totalCount)> GetPagedAllAsync(
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var accounts = _dbContext.Accounts.AsNoTracking();

        var totalCount = await accounts.CountAsync(ct);

        return (
            await accounts.OrderBy(a => a.CreatedAt).Skip(offset).Take(limit).ToListAsync(ct),
            totalCount
        );
    }

    public void Remove(Account account)
    {
        ArgumentNullException.ThrowIfNull(account, nameof(account));

        base.Remove(account);
    }

    public async Task<bool> AnyWithUsernameAsync(string username, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        return await _dbContext.Accounts.AnyAsync(
            a => EF.Functions.Collate(a.Username, CaseInsensitiveCollation) == username,
            ct
        );
    }
}
