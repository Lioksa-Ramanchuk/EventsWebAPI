namespace Events.Domain.Constants;

public static class AuthPolicies
{
    public const string AdministratorPolicy = nameof(AdministratorPolicy);
    public const string ParticipantPolicy = nameof(ParticipantPolicy);
    public const string AccountOwnerOrAdministratorPolicy = nameof(
        AccountOwnerOrAdministratorPolicy
    );
    public const string NotificationOwnerOrAdministratorPolicy = nameof(
        NotificationOwnerOrAdministratorPolicy
    );
    public const string ParticipantOwnerOrAdministratorPolicy = nameof(
        ParticipantOwnerOrAdministratorPolicy
    );
}
