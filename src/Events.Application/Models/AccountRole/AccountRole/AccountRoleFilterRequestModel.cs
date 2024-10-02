using Events.Application.Models.Common;

namespace Events.Application.Models.AccountRole.AccountRole;

public record AccountRoleFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
