using CsvHelper.Configuration;
using FlightScheduleDetector.Domain.Entities;

namespace FlightScheduleDetector.Infrastructure.Mappings
{
    public sealed class FlightMap : ClassMap<Flight>
    {
        public FlightMap()
        {
            Map(m => m.FlightId).Name("flight_id");
            Map(m => m.RouteId).Name("route_id");
            Map(m => m.DepartureTime).Name("departure_time");
            Map(m => m.ArrivalTime).Name("arrival_time");
            Map(m => m.AirlineId).Name("airline_id");
        }
    }
}