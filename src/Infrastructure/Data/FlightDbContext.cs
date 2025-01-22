using FlightScheduleDetector.Domain.Entities;
using FlightScheduleDetector.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FlightScheduleDetector.Infrastructure.Data;

public class FlightDbContext : DbContext
{
    public DbSet<Route> Routes { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new RouteConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
    }
}