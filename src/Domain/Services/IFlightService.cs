using FlightScheduleDetector.Domain.Models;

namespace FlightScheduleDetector.Domain.Services;

public interface IFlightService
{
    Task<List<FlightResult>> ProcessFlights(DateTime startDate, DateTime endDate, int agencyId);
}