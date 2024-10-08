using Events.Application.Models.Common;

namespace Events.Application.Models.Role;

public class RoleResponseModel : BaseResponseModel
{
    public string Title { get; set; } = null!;
}
