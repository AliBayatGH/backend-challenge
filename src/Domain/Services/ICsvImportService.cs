namespace FlightScheduleDetector.Domain.Services;

public interface ICsvImportService
{
    Task ImportDataAsync(string routesPath, string flightsPath, string subscriptionsPath);
}