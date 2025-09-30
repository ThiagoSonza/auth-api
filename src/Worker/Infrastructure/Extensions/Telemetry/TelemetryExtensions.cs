using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SharedKernel;

namespace Worker.Infrastructure.Extensions.Telemetry;

public static class TelemetryExtensions
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services, HostApplicationBuilder host)
    {
        var sp = services.BuildServiceProvider();
        var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value.Telemetry;

        var serviceName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()!.Title ?? "Unknown Service";
        var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        var endpoint = settings.Endpoint;

        var activitySource = new ActivitySource(serviceName);
        services.AddSingleton(activitySource);

        void configureResource(ResourceBuilder r) => r.AddService(
            serviceName: serviceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: Environment.MachineName);

        var logger = new LoggerConfiguration()
            .MinimumLevel.Is(GetLogLevel(settings.MinimumLevel))
            .MinimumLevel.ControlledBy(new LoggingLevelSwitch(GetLogLevel(settings.MinimumLevel)))
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ServiceName", serviceName)
            .Enrich.WithProperty("ServiceVersion", serviceVersion)
            .Enrich.WithProperty("Environment", host.Environment.EnvironmentName)
            .Enrich.WithProperty("HostName", Environment.MachineName)
            .Enrich.With(new OpenTelemetryEnricher())
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = endpoint;
                options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = serviceName
                };
            })
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger, dispose: true);
        });

        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
                tracerProviderBuilder
                    .AddSource(serviceName)
                    .ConfigureResource(configureResource)
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(endpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    }))
            .WithMetrics(metricsProviderBuilder =>
                metricsProviderBuilder
                    .AddAspNetCoreInstrumentation()
                    .ConfigureResource(configureResource)
                    .AddMeter(serviceName)
                    .AddMeter("Microsoft.AspNetCore.Http")
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("Microsoft.Data.SqlClient")
                    .AddMeter("Microsoft.EntityFrameworkCore")
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(endpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    }));

        return services;
    }

    private static LogEventLevel GetLogLevel(string minimumLevel)
    {
        var nivel = minimumLevel.ToUpperInvariant();

        return nivel switch
        {
            "VERBOSE" => LogEventLevel.Verbose,
            "DEBUG" => LogEventLevel.Debug,
            "INFORMATION" => LogEventLevel.Information,
            "WARNING" => LogEventLevel.Warning,
            "ERROR" => LogEventLevel.Error,
            "FATAL" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information,
        };
    }
}
