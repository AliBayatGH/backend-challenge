namespace FlightScheduleDetector.Domain.Services;

public interface ICsvExportService
{
    Task ExportResultsAsync<T>(IEnumerable<T> records, string path);
}