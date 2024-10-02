using Events.Application.Models.Common;

namespace Events.Application.Models.Role;

public class RoleResponseModel : BaseResponseModel
{
    public RoleResponseModel()
        : base() { }

    public RoleResponseModel(Guid id, string title)
        : base(id)
    {
        Title = title;
    }

    public string Title { get; set; } = null!;
}
