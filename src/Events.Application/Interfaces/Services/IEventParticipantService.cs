using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.EventParticipant;

namespace Events.Application.Interfaces.Services;

public interface IEventParticipantService
{
    Task<EventParticipantResponseModel> GetEventParticipantByKeyAsModelAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task<PagedResponseModel<EventParticipantResponseModel>> GetEventParticipantsAsModelsAsync(
        Guid eventId,
        EventParticipantFilterRequestModel filterModel,
        CancellationToken ct = default
    );
    Task<EventParticipantResponseModel> RegisterEventParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task UnregisterEventParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
}
