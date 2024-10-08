using Events.Application.Models.Common;

namespace Events.Application.Models.Event;

public class EventFilterRequestModel : BaseFilterRequestModel
{
    public DateTime? StartEventDate { get; set; }
    public DateTime? EndEventDate { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
}
