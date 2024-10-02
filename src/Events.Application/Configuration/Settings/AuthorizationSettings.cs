namespace Events.Application.Configuration.Settings;

public class AuthorizationSettings
{
    public RoleTitles RoleTitles { get; set; } = null!;
}

public class RoleTitles
{
    public string Administrator { get; set; } = null!;
    public string Participant { get; set; } = null!;
}
