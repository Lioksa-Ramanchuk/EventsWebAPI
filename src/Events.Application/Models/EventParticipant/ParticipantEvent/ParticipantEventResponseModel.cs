using Events.Application.Models.Event;

namespace Events.Application.Models.EventParticipant.ParticipantEvent;

public class ParticipantEventResponseModel : BaseEventParticipantRegistrationResponseModel
{
    public EventResponseModel Event { get; set; } = null!;
}
