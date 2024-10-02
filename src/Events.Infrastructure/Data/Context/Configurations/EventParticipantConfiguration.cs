using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Data.Context.Configurations;

public class EventParticipantConfiguration() : IEntityTypeConfiguration<EventParticipant>
{
    public void Configure(EntityTypeBuilder<EventParticipant> entity)
    {
        entity.HasKey(ep => new { ep.EventId, ep.ParticipantId });

        entity.Property(ep => ep.RegistrationDate).IsRequired().HasDefaultValueNow();

        entity.Property(ep => ep.IsNotifiedToday).IsRequired().HasDefaultValue(false);

        entity
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasOne(ep => ep.Participant)
            .WithMany(p => p.EventParticipants)
            .HasForeignKey(ep => ep.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
