using CsvHelper;
using CsvHelper.Configuration;
using FlightScheduleDetector.Domain.Entities;
using FlightScheduleDetector.Domain.Services;
using FlightScheduleDetector.Infrastructure.Data;
using FlightScheduleDetector.Infrastructure.Mappings;
using System.Globalization;

namespace FlightScheduleDetector.Infrastructure.Services
{
    public class CsvImportService : ICsvImportService
    {
        private readonly FlightDbContext _context;

        public CsvImportService(FlightDbContext context)
        {
            _context = context;
        }

        public async Task ImportDataAsync(string routesPath, string flightsPath, string subscriptionsPath)
        {
            _context.ChangeTracker.Clear();
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null
            };

            if (!File.Exists(routesPath) || !File.Exists(flightsPath) || !File.Exists(subscriptionsPath))
            {
                throw new FileNotFoundException("One or more required CSV files not found.");
            }

            Dictionary<int, Route> routeCache = new();
            using (var reader = new StreamReader(routesPath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<RouteMap>();
                var routes = csv.GetRecords<Route>().ToList();
                foreach (var route in routes)
                {
                    routeCache[route.RouteId] = route;
                }
                await _context.Routes.AddRangeAsync(routes);
                await _context.SaveChangesAsync();
            }

            using (var reader = new StreamReader(flightsPath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<FlightMap>();
                var flights = csv.GetRecords<Flight>().ToList();
                foreach (var flight in flights)
                {
                    if (routeCache.TryGetValue(flight.RouteId, out var route))
                    {
                        flight.Route = route;
                    }
                }
                int batchSize = 1000; 
                for (int i = 0; i < flights.Count; i += batchSize)
                {
                    var batch = flights.Skip(i).Take(batchSize);
                    await _context.Flights.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();
                }

            }

            using (var reader = new StreamReader(subscriptionsPath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SubscriptionMap>();
                var subscriptions = csv.GetRecords<Subscription>().ToList();
                await _context.Subscriptions.AddRangeAsync(subscriptions);
                await _context.SaveChangesAsync();
            }
        }
    }
}