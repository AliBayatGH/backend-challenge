using FlightScheduleDetector.Domain.Entities;
using FlightScheduleDetector.Domain.Repositories;
using FlightScheduleDetector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightScheduleDetector.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightDbContext _context;

        public FlightRepository(FlightDbContext context)
        {
            _context = context;
        }

        // Get Flights in Date Range with Subscription Filter
        public async Task<IEnumerable<Flight>> GetFlightsInDateRange(DateTime startDate, DateTime endDate, int agencyId)
        {
            // 1. Filters flights within date range
            // 2. Joins with Routes (Include)
            // 3. Matches agency subscriptions using subquery
            // 4. Uses composite index (RouteId, DepartureTime)
            return await _context.Flights
                .Include(f => f.Route)
                .Where(f => f.DepartureTime >= startDate &&
                           f.DepartureTime <= endDate &&
                           _context.Subscriptions.Any(s =>
                               s.AgencyId == agencyId &&
                               s.OriginCityId == f.Route.OriginCityId &&
                               s.DestinationCityId == f.Route.DestinationCityId))
                .ToListAsync();
        }

        public async Task<List<Flight>> GetCorrespondingFlights(Flight flight, TimeSpan tolerance, int weekOffset)
        {
            // Calculate the target date range for comparison
            var targetDepartureTime = flight.DepartureTime.AddDays(weekOffset);
            var startRange = targetDepartureTime - tolerance;
            var endRange = targetDepartureTime + tolerance;

            return await _context.Flights
                .Where(f => f.AirlineId == flight.AirlineId &&
                            f.Route.OriginCityId == flight.Route.OriginCityId &&
                            f.Route.DestinationCityId == flight.Route.DestinationCityId &&
                            f.DepartureTime >= startRange &&
                            f.DepartureTime <= endRange)
                .ToListAsync();
        }
    }
}