namespace Events.Application.Configuration.Settings;

public class ValidationSettings
{
    public int StringMaxLength { get; set; }

    public int MaxRoleTitleLength { get; set; }

    public int AccountUsernameMaxLength { get; set; }
    public string AccountUsernameFormat { get; set; } = null!;
    public int AccountPasswordMinLength { get; set; }

    public int ParticipantFirstNameMaxLength { get; set; }
    public int ParticipantLastNameMaxLength { get; set; }
    public int ParticipantEmailMaxLength { get; set; }

    public int EventTitleMaxLength { get; set; }
    public int EventLocationMaxLength { get; set; }
    public int EventCategoryMaxLength { get; set; }
    public int MinEventMaxParticipantsCount { get; set; }

    public int MaxNotificationMessageLength { get; set; }

    public int MaxFilterLimit { get; set; }
}
