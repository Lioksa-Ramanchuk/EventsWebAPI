using Events.Application.Models.Common;

namespace Events.Application.Models.Event;

public record EventFilterRequestModel(
    DateTime? StartEventDate,
    DateTime? EndEventDate,
    string? Location,
    string? Category,
    int? Offset,
    int? Limit
) : BaseFilterRequestModel(Offset, Limit);
