namespace Events.Application.Models.Event;

public class EventUpdateRequestModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? EventDate { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
    public int? MaxParticipantsCount { get; set; }
}
