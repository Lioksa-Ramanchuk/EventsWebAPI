using Events.Application.Models.Common;

namespace Events.Application.Models.EventParticipant.ParticipantEvent;

public record ParticipantEventFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
