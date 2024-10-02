using Events.Application.Models.Event;

namespace Events.Application.Models.EventParticipant.ParticipantEvent;

public class ParticipantEventResponseModel : BaseEventParticipantRegistrationResponseModel
{
    public ParticipantEventResponseModel()
        : base() { }

    public ParticipantEventResponseModel(DateTime registrationDate, EventResponseModel eventModel)
        : base(registrationDate)
    {
        RegistrationDate = registrationDate;
        Event = eventModel;
    }

    public EventResponseModel Event { get; set; } = null!;
}
