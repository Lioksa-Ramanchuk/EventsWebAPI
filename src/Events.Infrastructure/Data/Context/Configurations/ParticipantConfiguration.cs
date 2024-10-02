using Events.Application.Configuration.Settings;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Data.Context.Configurations;

public class ParticipantConfiguration(IOptions<AppSettings> appSettings)
    : IEntityTypeConfiguration<Participant>
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public void Configure(EntityTypeBuilder<Participant> entity)
    {
        entity
            .Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(_validationSettings.ParticipantFirstNameMaxLength);

        entity
            .Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(_validationSettings.ParticipantLastNameMaxLength);

        entity.Property(p => p.BirthDate).IsRequired();

        entity
            .Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(_validationSettings.ParticipantEmailMaxLength);
    }
}
