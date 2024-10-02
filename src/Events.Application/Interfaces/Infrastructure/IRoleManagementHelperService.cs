namespace Events.Application.Interfaces.Infrastructure;

public interface IRoleManagementHelperService
{
    string AdministratorRoleTitle { get; }
    string ParticipantRoleTitle { get; }

    Task<List<string>> GetInitialParticipantRoleTitlesAsync(CancellationToken ct = default);
    Task<bool> IsLastAdministratorByIdAsync(Guid accountId, CancellationToken ct = default);
}
