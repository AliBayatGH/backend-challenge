using FlightScheduleDetector.Domain.Models;
using FlightScheduleDetector.Domain.Repositories;
using FlightScheduleDetector.Domain.Services;

namespace FlightScheduleDetector.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repository;

        public FlightService(IFlightRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FlightResult>> ProcessFlights(DateTime startDate, DateTime endDate, int agencyId)
        {
            var results = new List<FlightResult>();

            // Step 1: Get all relevant flights
            var flights = await _repository.GetFlightsInDateRange(startDate, endDate, agencyId);

            // Step 2: Process each flight
            foreach (var flight in flights)
            {
                // Step 3: Check for corresponding flights in previous week
                var previousWeekFlights = await _repository.GetCorrespondingFlights(flight, TimeSpan.FromMinutes(30), -7);

                // Step 3.1: Check for corresponding flights in next week
                var nextWeekFlights = await _repository.GetCorrespondingFlights(flight, TimeSpan.FromMinutes(30), 7);

                // Step 4: Determine flight status
                if (!previousWeekFlights.Any())
                {
                    // New flight detected
                    results.Add(new FlightResult
                    {
                        FlightId = flight.FlightId,
                        OriginCityId = flight.Route.OriginCityId,
                        DestinationCityId = flight.Route.DestinationCityId,
                        DepartureTime = flight.DepartureTime,
                        ArrivalTime = flight.ArrivalTime,
                        AirlineId = flight.AirlineId,
                        Status = "New"
                    });
                }

                if (!nextWeekFlights.Any())
                {
                    // Discontinued flight detected
                    results.Add(new FlightResult
                    {
                        FlightId = flight.FlightId,
                        OriginCityId = flight.Route.OriginCityId,
                        DestinationCityId = flight.Route.DestinationCityId,
                        DepartureTime = flight.DepartureTime,
                        ArrivalTime = flight.ArrivalTime,
                        AirlineId = flight.AirlineId,
                        Status = "Discontinued"
                    });
                }
            }

            return results;
        }
    }
} 