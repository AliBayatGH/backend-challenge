using CsvHelper;
using FlightScheduleDetector.Domain.Services;
using System.Globalization;

namespace FlightScheduleDetector.Infrastructure.Services
{
    public class CsvExportService : ICsvExportService
    {
        public async Task ExportResultsAsync<T>(IEnumerable<T> records, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(records);
        }
    }
}