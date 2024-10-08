using Events.Application.Models.Participant;

namespace Events.Application.Models.EventParticipant.EventParticipant;

public class EventParticipantResponseModel : BaseEventParticipantRegistrationResponseModel
{
    public ParticipantResponseModel Participant { get; set; } = null!;
}
