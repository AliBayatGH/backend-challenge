namespace FlightScheduleDetector.Domain.Models;

public class FlightResult
{
    public int FlightId { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AirlineId { get; set; }
    public string Status { get; set; } = string.Empty;
}