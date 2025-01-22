using FlightScheduleDetector.Domain.Entities;

namespace FlightScheduleDetector.Domain.Repositories;

public interface IFlightRepository
{
    Task<IEnumerable<Flight>> GetFlightsInDateRange(DateTime startDate, DateTime endDate, int agencyId);
    Task<List<Flight>> GetCorrespondingFlights(Flight flight, TimeSpan tolerance, int weekOffset);
}