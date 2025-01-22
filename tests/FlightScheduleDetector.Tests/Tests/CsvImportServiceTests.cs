using Microsoft.EntityFrameworkCore;
using FlightScheduleDetector.Tests.Fixtures;
using FlightScheduleDetector.Infrastructure.Services;

namespace FlightScheduleDetector.Tests.Services;

public class CsvImportServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly string routesPath;
    private readonly string flightsPath;
    private readonly string subscriptionsPath;
    private readonly string _testDataPath;

    public CsvImportServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _testDataPath = Path.Combine(Path.GetTempPath(), $"FlightTestData_{Guid.NewGuid()}");

         routesPath = "Data/routes.csv";
         flightsPath = "Data/flights.csv";
         subscriptionsPath = "Data/subscriptions.csv";
    }

    [Fact]
    public async Task ImportDataAsync_WithValidData_ImportsSuccessfully()
    {
        // Arrange
        await _fixture.Context.Database.EnsureDeletedAsync();
        await _fixture.Context.Database.EnsureCreatedAsync();

        var service = new CsvImportService(_fixture.Context);


        try
        {
            // Act
            await service.ImportDataAsync(routesPath, flightsPath, subscriptionsPath);

            // Assert
            Assert.True(await _fixture.Context.Routes.AnyAsync());
            Assert.True(await _fixture.Context.Flights.AnyAsync());
            Assert.True(await _fixture.Context.Subscriptions.AnyAsync());

            // Verify data integrity
            var route = await _fixture.Context.Routes.FirstAsync();
            Assert.NotEqual(0, route.RouteId);
            Assert.NotEqual(0, route.OriginCityId);
            Assert.NotEqual(0, route.DestinationCityId);
        }
        finally
        {
            if (Directory.Exists(_testDataPath))
            {
                Directory.Delete(_testDataPath, true);
            }
        }
    }

    [Fact]
    public async Task ImportDataAsync_WithInvalidFilePath_ThrowsFileNotFoundException()
    {
        // Arrange
        var service = new CsvImportService(_fixture.Context);

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            service.ImportDataAsync("nonexistent.csv", "nonexistent.csv", "nonexistent.csv"));
    }
}