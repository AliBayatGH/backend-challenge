namespace FlightScheduleDetector.Domain.Services;

public interface IArgsParser
{
    (DateTime StartDate, DateTime EndDate, int AgencyId) ParseArgs(string[] args);
}