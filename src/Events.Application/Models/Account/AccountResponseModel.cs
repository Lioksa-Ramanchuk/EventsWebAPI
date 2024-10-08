using Events.Application.Models.Common;

namespace Events.Application.Models.Account;

public class AccountResponseModel : BaseResponseModel
{
    public string Username { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
