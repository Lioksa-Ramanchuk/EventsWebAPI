using Events.Application.Models.Event;

namespace Events.Tests.Common.Mocking.ModelCreators;

public static class EventModelCreator
{
    public static EventAddRequestModel CreateEventAddRequestModel(
        string title = "Event",
        string description = "Description",
        DateTime eventDate = default,
        string location = "Location",
        string category = "Category",
        int maxParticipantsCount = 100
    )
    {
        if (eventDate == default)
        {
            eventDate = DateTime.UtcNow.AddDays(1);
        }
        return new EventAddRequestModel(
            title,
            description,
            eventDate,
            location,
            category,
            maxParticipantsCount
        );
    }

    public static EventResponseModel CreateEventResponseModel(
        Guid id = default,
        string title = "Event",
        string description = "Description",
        DateTime eventDate = default,
        string location = "Location",
        string category = "Category",
        int maxParticipantsCount = 100,
        string? imageFileName = null,
        string? imageUrl = null,
        DateTime createdAt = default
    )
    {
        DateTime now = DateTime.UtcNow;
        if (id == default)
        {
            id = Guid.NewGuid();
        }
        if (eventDate == default)
        {
            eventDate = now.AddDays(1);
        }
        if (createdAt == default)
        {
            createdAt = now;
        }
        return new EventResponseModel(
            id,
            title,
            description,
            eventDate,
            location,
            category,
            maxParticipantsCount,
            imageFileName,
            imageUrl,
            createdAt
        );
    }
}
