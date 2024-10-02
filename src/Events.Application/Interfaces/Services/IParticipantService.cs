using Events.Application.Models.Common;
using Events.Application.Models.Participant;

namespace Events.Application.Interfaces.Services;

public interface IParticipantService
{
    Task<ParticipantResponseModel> GetParticipantByIdAsModelAsync(
        Guid participantId,
        CancellationToken ct = default
    );
    Task<PagedResponseModel<ParticipantResponseModel>> GetAllParticipantsAsModelsAsync(
        ParticipantFilterRequestModel filterModel,
        CancellationToken ct = default
    );
    Task<ParticipantResponseModel> UpdateParticipantAsync(
        Guid participantId,
        ParticipantUpdateRequestModel updateModel,
        CancellationToken ct = default
    );
}
