using Events.Application.Configuration.Settings;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Data.Context.Configurations;

public class EventConfiguration(IOptions<AppSettings> appSettings) : BaseEntityConfiguration<Event>
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public override void Configure(EntityTypeBuilder<Event> entity)
    {
        base.Configure(entity);

        entity
            .Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(_validationSettings.EventTitleMaxLength);

        entity.Property(e => e.Description).IsRequired();

        entity.Property(e => e.EventDate).IsRequired();

        entity
            .Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(_validationSettings.EventLocationMaxLength);

        entity
            .Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(_validationSettings.EventCategoryMaxLength);

        entity.Property(e => e.MaxParticipantsCount).IsRequired();

        entity.Property(e => e.ImageFileName).IsRequired(false);

        entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueNow();

        entity.HasIndex(e => e.Title).IsUnique();
    }
}
