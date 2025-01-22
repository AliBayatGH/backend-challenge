using CsvHelper.Configuration;
using FlightScheduleDetector.Domain.Entities;

namespace FlightScheduleDetector.Infrastructure.Mappings
{
    public sealed class SubscriptionMap : ClassMap<Subscription>
    {
        public SubscriptionMap()
        {
            Map(m => m.AgencyId).Name("agency_id");
            Map(m => m.OriginCityId).Name("origin_city_id");
            Map(m => m.DestinationCityId).Name("destination_city_id");
        }
    }
}