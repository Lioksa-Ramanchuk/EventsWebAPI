using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Data.Context.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.HasKey(rt => rt.Token);

        entity.Property(rt => rt.Token).IsRequired();

        entity.Property(rt => rt.CreatedAt).IsRequired().HasDefaultValueNow();

        entity.Property(rt => rt.ExpiresAt).IsRequired();

        entity.Ignore(rt => rt.IsExpired);
        entity.Ignore(rt => rt.IsActive);
    }
}
