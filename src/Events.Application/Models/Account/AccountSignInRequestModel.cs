namespace Events.Application.Models.Account;

public class AccountSignInRequestModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
