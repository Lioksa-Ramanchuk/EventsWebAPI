using Events.Application.Models.Common;

namespace Events.Application.Models.Account;

public class AccountResponseModel : BaseResponseModel
{
    public AccountResponseModel()
        : base() { }

    public AccountResponseModel(Guid id, string username, DateTime createdAt)
        : base(id)
    {
        Username = username;
        CreatedAt = createdAt;
    }

    public string Username { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
