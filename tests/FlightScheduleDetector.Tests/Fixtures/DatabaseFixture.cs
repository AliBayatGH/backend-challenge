using FlightScheduleDetector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightScheduleDetector.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public FlightDbContext Context { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<FlightDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        Context = new FlightDbContext(options);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}