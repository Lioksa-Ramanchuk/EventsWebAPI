namespace Events.Application.Models.EventParticipant;

public class BaseEventParticipantRegistrationResponseModel
{
    public BaseEventParticipantRegistrationResponseModel() { }

    public BaseEventParticipantRegistrationResponseModel(DateTime registrationDate)
    {
        RegistrationDate = registrationDate;
    }

    public DateTime RegistrationDate { get; set; }
}
