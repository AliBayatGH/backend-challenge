using FlightScheduleDetector.Domain.Services;
using FlightScheduleDetector.Infrastructure.Data;
using FlightScheduleDetector.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace FlightScheduleDetector.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var stopwatch = new Stopwatch();

            // Setup dependency injection
            var services = new ServiceCollection();
            services.ConfigureServices(configuration.GetConnectionString("DefaultConnection")!);

            using var serviceProvider = services.BuildServiceProvider();
            var argsParser = serviceProvider.GetRequiredService<IArgsParser>();

            long importTime = 0; // Declare importTime outside the block
            long processingTime = 0; // Declare processingTime outside the block

            try
            {
                var (startDate, endDate, agencyId) = argsParser.ParseArgs(args);

                var context = serviceProvider.GetRequiredService<FlightDbContext>();

                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Import CSV data if database is empty
                if (!context.Routes.Any())
                {
                    var csvImportService = serviceProvider.GetRequiredService<ICsvImportService>();
                    var routesPath = configuration["CsvFiles:RoutesPath"]!;
                    var flightsPath = configuration["CsvFiles:FlightsPath"]!;
                    var subscriptionsPath = configuration["CsvFiles:SubscriptionsPath"]!;

                    // Import data
                    Console.WriteLine("Importing data...");
                    stopwatch.Start();
                    await csvImportService.ImportDataAsync(routesPath, flightsPath, subscriptionsPath);
                    importTime = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine($"Data import completed in {importTime}ms");
                }

                var flightService = serviceProvider.GetRequiredService<IFlightService>();

                // Process flights
                stopwatch.Restart();
                var results = await flightService.ProcessFlights(startDate, endDate, agencyId);
                processingTime = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Flight processing completed in {processingTime}ms");

                // Export results
                var outputPath = "Data/results.csv";
                var csvExportService = serviceProvider.GetRequiredService<ICsvExportService>();
                await csvExportService.ExportResultsAsync(results, outputPath);
                Console.WriteLine($"Results exported to {outputPath}");

                // Performance summary
                Console.WriteLine("\nPerformance Metrics:");
                Console.WriteLine($"Data Import: {importTime}ms");
                Console.WriteLine($"Processing: {processingTime}ms");
                Console.WriteLine($"Total: {importTime + processingTime}ms");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}