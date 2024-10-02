using Events.Application.Configuration.Settings;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Data.Context.Configurations;

public class RoleConfiguration(IOptions<AppSettings> appSettings) : BaseEntityConfiguration<Role>
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public override void Configure(EntityTypeBuilder<Role> entity)
    {
        base.Configure(entity);

        entity
            .Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(_validationSettings.MaxRoleTitleLength);

        entity.HasIndex(r => r.Title).IsUnique();
    }
}
