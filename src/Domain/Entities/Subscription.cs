namespace FlightScheduleDetector.Domain.Entities;

public class Subscription
{
    public int AgencyId { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }
}