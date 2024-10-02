using Events.Application.Models.Common;

namespace Events.Application.Models.Event;

public class EventResponseModel : BaseResponseModel
{
    public EventResponseModel()
        : base() { }

    public EventResponseModel(
        Guid id,
        string title,
        string description,
        DateTime eventDate,
        string location,
        string category,
        int maxParticipantsCount,
        string? imageFileName,
        string? imageUrl,
        DateTime createdAt
    )
        : base(id)
    {
        Title = title;
        Description = description;
        EventDate = eventDate;
        Location = location;
        Category = category;
        MaxParticipantsCount = maxParticipantsCount;
        ImageFileName = imageFileName;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
    }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int MaxParticipantsCount { get; set; }
    public string? ImageFileName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
