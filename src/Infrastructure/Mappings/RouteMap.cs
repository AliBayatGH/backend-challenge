using CsvHelper.Configuration;
using FlightScheduleDetector.Domain.Entities;

namespace FlightScheduleDetector.Infrastructure.Mappings
{
    public sealed class RouteMap : ClassMap<Route>
    {
        public RouteMap()
        {
            Map(m => m.RouteId).Name("route_id");
            Map(m => m.OriginCityId).Name("origin_city_id");
            Map(m => m.DestinationCityId).Name("destination_city_id");
            Map(m => m.DepartureDate).Name("departure_date");
        }
    }
}