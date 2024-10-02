using Events.Application.Configuration.Settings;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Events.Infrastructure.Data.Context.Configurations;

public class AccountConfiguration(IOptions<AppSettings> appSettings)
    : BaseEntityConfiguration<Account>
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public override void Configure(EntityTypeBuilder<Account> entity)
    {
        base.Configure(entity);

        entity
            .Property(a => a.Username)
            .IsRequired()
            .HasMaxLength(_validationSettings.AccountUsernameMaxLength);

        entity.Property(a => a.Password).IsRequired();

        entity.Property(a => a.CreatedAt).IsRequired().HasDefaultValueNow();

        entity.HasIndex(a => a.Username).IsUnique();

        entity
            .HasMany(a => a.RefreshTokens)
            .WithOne(rt => rt.Account)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasMany(a => a.Notifications)
            .WithOne(n => n.Account)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
