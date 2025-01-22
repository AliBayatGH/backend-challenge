using FlightScheduleDetector.Application.Services;
using FlightScheduleDetector.Domain.Repositories;
using FlightScheduleDetector.Domain.Services;
using FlightScheduleDetector.Infrastructure.Data;
using FlightScheduleDetector.Infrastructure.Repositories;
using FlightScheduleDetector.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlightScheduleDetector.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FlightDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<ICsvImportService, CsvImportService>();
            services.AddScoped<ICsvExportService, CsvExportService>();
            services.AddTransient<IArgsParser, ArgsParser>();

            return services;
        }
    }
} 