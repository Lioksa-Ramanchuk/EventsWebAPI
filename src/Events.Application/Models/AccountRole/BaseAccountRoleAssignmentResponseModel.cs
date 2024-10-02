namespace Events.Application.Models.AccountRole;

public class BaseAccountRoleAssignmentResponseModel
{
    public BaseAccountRoleAssignmentResponseModel() { }

    public BaseAccountRoleAssignmentResponseModel(DateTime assignedAt)
    {
        AssignedAt = assignedAt;
    }

    public DateTime AssignedAt { get; set; }
}
