using Events.Application.Models.Participant;

namespace Events.Application.Models.EventParticipant.EventParticipant;

public class EventParticipantResponseModel : BaseEventParticipantRegistrationResponseModel
{
    public EventParticipantResponseModel()
        : base() { }

    public EventParticipantResponseModel(
        DateTime registrationDate,
        ParticipantResponseModel participantModel
    )
        : base(registrationDate)
    {
        RegistrationDate = registrationDate;
        Participant = participantModel;
    }

    public ParticipantResponseModel Participant { get; set; } = null!;
}
