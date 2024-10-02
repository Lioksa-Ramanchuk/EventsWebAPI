using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.ParticipantEvent;

namespace Events.Application.Interfaces.Services;

public interface IParticipantEventService
{
    Task<ParticipantEventResponseModel> GetParticipantEventByKeyAsModelAsync(
        Guid participantId,
        Guid eventId,
        CancellationToken ct = default
    );
    Task<PagedResponseModel<ParticipantEventResponseModel>> GetParticipantEventsAsModelsAsync(
        Guid participantId,
        ParticipantEventFilterRequestModel filterModel,
        CancellationToken ct = default
    );
}
