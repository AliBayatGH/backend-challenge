using FlightScheduleDetector.Presentation;

namespace FlightScheduleDetector.Tests.Tests;

public class ProgramTests
{
    private const string AppSettingsFileName = "appsettings.json";

    [Fact]
    public async Task Test_Main_WithValidArguments()
    {
        // Arrange
        var args = new[] { "2025-01-15", "2025-01-17", "1" };

        //EnsureAppSettingsFileExists();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        await Program.Main(args);

        // Assert
        var output = sw.ToString();
        Assert.Contains("Flight processing completed", output);
    }

    [Fact]
    public async Task Test_Main_WithInvalidArguments_ShouldLogError()
    {
        // Arrange
        var args = new[] { "invalid-date", "2025-01-31", "14" };

        //EnsureAppSettingsFileExists();

        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        await Program.Main(args);

        // Assert
        var output = sw.ToString();
        Assert.Contains("Error: Invalid date format", output);
    }

    private static void EnsureAppSettingsFileExists()
    {
        if (!File.Exists(AppSettingsFileName))
        {
            File.WriteAllText(AppSettingsFileName, GenerateMockAppSettings());
        }
    }

    private static string GenerateMockAppSettings()
    {
        return @"
        {
            ""ConnectionStrings"": {
                ""DefaultConnection"": ""DataSource=InMemoryDb;Mode=Memory""
            },
            ""CsvFiles"": {
                ""RoutesPath"": ""Data/routes.csv"",
                ""FlightsPath"": ""Data/flights.csv"",
                ""SubscriptionsPath"": ""Data/subscriptions.csv""
            }
        }";
    }
}