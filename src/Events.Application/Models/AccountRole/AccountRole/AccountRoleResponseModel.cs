using Events.Application.Models.Role;

namespace Events.Application.Models.AccountRole.AccountRole;

public class AccountRoleResponseModel : BaseAccountRoleAssignmentResponseModel
{
    public RoleResponseModel Role { get; set; } = null!;
}
