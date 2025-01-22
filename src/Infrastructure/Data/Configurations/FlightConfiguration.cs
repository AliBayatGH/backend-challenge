using FlightScheduleDetector.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightScheduleDetector.Infrastructure.Data.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(f => f.FlightId);
        builder.Property(r => r.FlightId).ValueGeneratedNever();

        builder.HasOne(f => f.Route)
              .WithMany(r => r.Flights)
              .HasForeignKey(f => f.RouteId);

        builder.HasIndex(f => f.DepartureTime);
        builder.HasIndex(f => new { f.RouteId, f.DepartureTime });
    }
}