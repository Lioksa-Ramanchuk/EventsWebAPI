using Events.Application.Models.Role;

namespace Events.Application.Models.AccountRole.AccountRole;

public class AccountRoleResponseModel : BaseAccountRoleAssignmentResponseModel
{
    public AccountRoleResponseModel()
        : base() { }

    public AccountRoleResponseModel(DateTime assignedAt, RoleResponseModel roleModel)
        : base(assignedAt)
    {
        Role = roleModel;
    }

    public RoleResponseModel Role { get; set; } = null!;
}
