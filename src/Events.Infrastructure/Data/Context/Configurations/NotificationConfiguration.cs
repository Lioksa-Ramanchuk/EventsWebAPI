using Events.Application.Configuration.Settings;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Data.Context.Configurations;

public class NotificationConfiguration(IOptions<AppSettings> appSettings)
    : BaseEntityConfiguration<Notification>
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public override void Configure(EntityTypeBuilder<Notification> entity)
    {
        base.Configure(entity);

        entity
            .Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(_validationSettings.MaxNotificationMessageLength);

        entity.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);

        entity.Property(n => n.SentAt).IsRequired().HasDefaultValueNow();
    }
}
