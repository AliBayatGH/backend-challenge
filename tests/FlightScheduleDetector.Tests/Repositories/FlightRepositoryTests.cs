using FlightScheduleDetector.Domain.Entities;
using FlightScheduleDetector.Infrastructure.Data;
using FlightScheduleDetector.Infrastructure.Repositories;
using FlightScheduleDetector.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace FlightScheduleDetector.Tests.Repositories;

public class FlightRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DbContextOptions<FlightDbContext> _options;

    public FlightRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<FlightDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetFlightsInDateRange_ReturnsCorrectFlights()
    {
        // Arrange
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(7);

        using (var context = new FlightDbContext(_options))
        {
            await SeedTestData(context);
        }

        // Act
        using (var context = new FlightDbContext(_options))
        {
            var repository = new FlightRepository(context);
            var flights = await repository.GetFlightsInDateRange(startDate, endDate, 1);

            // Assert
            Assert.NotEmpty(flights);
            Assert.All(flights, f =>
                Assert.True(f.DepartureTime >= startDate && f.DepartureTime <= endDate));
        }
    }


    private async Task SeedTestData(FlightDbContext context)
    {
        var route = new Route
        {
            RouteId = 1,
            OriginCityId = 1,
            DestinationCityId = 2,
            DepartureDate = DateTime.Now.Date
        };

        var flight = new Flight
        {
            FlightId = 1,
            RouteId = 1,
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(2),
            AirlineId = 1,
            Route = route
        };

        var subscription = new Subscription
        {
            AgencyId = 1,
            OriginCityId = 1,
            DestinationCityId = 2
        };

        await context.Routes.AddAsync(route);
        await context.Flights.AddAsync(flight);
        await context.Subscriptions.AddAsync(subscription);
        await context.SaveChangesAsync();
    }
}