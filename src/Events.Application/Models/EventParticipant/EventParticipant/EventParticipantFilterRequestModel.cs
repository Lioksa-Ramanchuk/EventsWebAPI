using Events.Application.Models.Common;

namespace Events.Application.Models.EventParticipant.EventParticipant;

public record EventParticipantFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
