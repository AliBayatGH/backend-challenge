using FlightScheduleDetector.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightScheduleDetector.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => new { s.AgencyId, s.OriginCityId, s.DestinationCityId });
        builder.HasIndex(s => s.AgencyId);
    }
}