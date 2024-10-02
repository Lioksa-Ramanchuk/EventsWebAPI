using Events.Application.Models.Common;

namespace Events.Application.Models.Participant;

public record ParticipantFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
