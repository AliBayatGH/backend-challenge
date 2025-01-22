using FlightScheduleDetector.Domain.Services;
using System.Globalization;

namespace FlightScheduleDetector.Application.Services
{
    public class ArgsParser : IArgsParser
    {
        public (DateTime StartDate, DateTime EndDate, int AgencyId) ParseArgs(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Usage: Program.exe <StartDate> <EndDate> <AgencyID>");
            }

            if (!DateTime.TryParseExact(args[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate)
                || !DateTime.TryParseExact(args[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate))
            {
                throw new ArgumentException("Invalid date format. Use yyyy-MM-dd.");
            }

            if (!int.TryParse(args[2], out var agencyId))
            {
                throw new ArgumentException("Invalid agency id.");
            }

            return (startDate, endDate, agencyId);
        }
    }
} 