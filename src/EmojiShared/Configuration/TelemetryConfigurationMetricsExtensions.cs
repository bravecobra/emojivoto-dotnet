using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace EmojiShared.Configuration;

public static class TelemetryConfigurationMetricsExtensions
{
    public static IServiceCollection AddCustomMetrics(this IServiceCollection services,
        IConfiguration configuration, ResourceBuilder resourceBuilder, string? meterName = null)
    {
        services.AddOpenTelemetryMetrics(options =>
        {
            options.SetResourceBuilder(resourceBuilder)
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddProcessInstrumentation()
                .AddEventCountersInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.AddEventSources();
                })
                .AddMeter(meterName ?? Assembly.GetEntryAssembly()?.GetName().Name);

            var metricsExporter = configuration.GetValue<string>("UseMetricsExporter")!.ToLowerInvariant();
            switch (metricsExporter)
            {
                case "prometheus":
                    options.AddPrometheusExporter(exporterOptions =>
                    {
                        exporterOptions.StartHttpListener = true;
                    });
                    break;
                case "otlp":
                    options.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                        otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Otlp:Endpoint") + "/v1/metrics");
                        otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
                    });
                    break;
                default:
                    options.AddConsoleExporter();
                    break;
            }
        });
        return services;
    }
}