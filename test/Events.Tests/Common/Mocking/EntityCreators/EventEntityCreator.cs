using System.Data;
using Events.Domain.Entities;

namespace Events.Tests.Common.Mocking.EntityCreators;

public static class EventEntityCreator
{
    public static Event Create(
        Guid id = default,
        string title = "Event",
        string description = "Description",
        DateTime eventDate = default,
        string location = "Location",
        string category = "Category",
        int maxParticipantsCount = 100,
        string? imageFileName = null,
        DateTime createdAt = default,
        IEnumerable<Participant>? participants = null
    )
    {
        DateTime now = DateTime.Now;
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

        return new Event
        {
            Id = id,
            Title = title,
            Description = description,
            EventDate = eventDate,
            Location = location,
            Category = category,
            MaxParticipantsCount = maxParticipantsCount,
            ImageFileName = imageFileName,
            CreatedAt = createdAt,

            EventParticipants =
                participants?.Select(p => new EventParticipant { Participant = p }).ToList() ?? [],
        };
    }
}
