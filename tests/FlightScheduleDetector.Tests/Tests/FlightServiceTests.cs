using FlightScheduleDetector.Application.Services;
using FlightScheduleDetector.Domain.Repositories;
using FlightScheduleDetector.Domain.Services;
using FlightScheduleDetector.Infrastructure.Repositories;
using FlightScheduleDetector.Infrastructure.Services;
using FlightScheduleDetector.Tests.Fixtures;
using System.Diagnostics;

namespace FlightScheduleDetector.Tests.Services;

public class FlightServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly string _testDataPath;
    private readonly IFlightRepository _repository;
    private readonly IFlightService _service;
    private readonly string _routesPath;
    private readonly string _flightsPath;
    private readonly string _subscriptionsPath;

    public FlightServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _testDataPath = Path.Combine(Path.GetTempPath(), $"FlightTestData_{Guid.NewGuid()}");
        _repository = new FlightRepository(_fixture.Context);
        _service = new FlightService(_repository);

        _routesPath = "Data/routes.csv";
        _flightsPath = "Data/flights.csv";
        _subscriptionsPath = "Data/subscriptions.csv";
    }

    [Fact]
    public async Task ProcessFlights_WithLargeDataset_PerformsEfficiently()
    {
        // Arrange
        await _fixture.Context.Database.EnsureDeletedAsync();
        await _fixture.Context.Database.EnsureCreatedAsync();

        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(7);

        try
        {
            // Import test data
            var csvImportService = new CsvImportService(_fixture.Context);
            await csvImportService.ImportDataAsync(_routesPath, _flightsPath, _subscriptionsPath);

            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await _service.ProcessFlights(startDate, endDate, 1);

            stopwatch.Stop();

            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 5000,
                $"Processing took too long: {stopwatch.ElapsedMilliseconds}ms");
            Assert.NotNull(result);
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
    public async Task ProcessFlights_WithNoFlights_ReturnsEmptyList()
    {
        // Arrange
        await _fixture.Context.Database.EnsureDeletedAsync();
        await _fixture.Context.Database.EnsureCreatedAsync();

        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(7);

        // Act
        var result = await _service.ProcessFlights(startDate, endDate, 1);

        // Assert
        Assert.Empty(result);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDataPath))
        {
            Directory.Delete(_testDataPath, true);
        }
    }
}