using Events.Application.Models.Common;

namespace Events.Application.Models.Account;

public record AccountFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
