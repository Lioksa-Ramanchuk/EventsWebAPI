using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Data.Context.Configurations;

public static class ConfigurationExtensions
{
    public static PropertyBuilder<DateTime> HasDefaultValueNow(
        this PropertyBuilder<DateTime> property
    )
    {
        return property.HasDefaultValueSql("getutcdate()");
    }

    public static PropertyBuilder<DateOnly> HasDefaultValueNow(
        this PropertyBuilder<DateOnly> property
    )
    {
        return property.HasDefaultValueSql("cast(getutcdate() as date)");
    }
}
