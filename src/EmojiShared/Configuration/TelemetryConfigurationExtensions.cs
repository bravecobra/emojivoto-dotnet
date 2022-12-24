using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

namespace EmojiShared.Configuration
{
    public static class TelemetryConfigurationExtensions
    {
        public static IServiceCollection AddCustomTelemetry(this IServiceCollection services,
            IConfiguration configuration, string[] sources, string? meterName = null)
        {
            services.Configure<AspNetCoreInstrumentationOptions>(configuration.GetSection("AspNetCoreInstrumentation"));
            services.AddOpenTelemetry()
                .ConfigureResource(builder => ResourceBuilderFactory.ConfigureResourceBuilder(builder))
                .WithTracing(options =>
                {
                    options
                        .SetSampler(new AlwaysOnSampler())
                        .AddGrpcCoreInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddHttpClientInstrumentation(clientInstrumentationOptions =>
                            clientInstrumentationOptions.RecordException = true)
                        .AddEntityFrameworkCoreInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.SetDbStatementForStoredProcedure = true;
                            instrumentationOptions.SetDbStatementForText = true;
                        })
                        .AddXRayTraceId()
                        .AddAWSInstrumentation()
                        .AddAspNetCoreInstrumentation(instrumentationOptions =>
                        {
                            instrumentationOptions.EnrichWithHttpRequest = EnrichWithHttpRequest;
                            instrumentationOptions.EnrichWithHttpResponse = EnrichWithHttpResponse;
                            instrumentationOptions.RecordException = true;
                            instrumentationOptions.EnableGrpcAspNetCoreSupport = true;
                        })
                        .AddSource(Assembly.GetEntryAssembly()?.GetName().Name)
                        .AddSource(sources);
                    var tracingExporter = configuration.GetValue<string>("UseTracingExporter")!.ToLowerInvariant();
                    switch (tracingExporter)
                    {
                        case "jaeger":
                            options.AddJaegerExporter();
                            services.Configure<JaegerExporterOptions>(configuration.GetSection("Jaeger"));

                            // Customize the HttpClient that will be used when JaegerExporter is configured for HTTP transport.
                            services.AddHttpClient("JaegerExporter", configureClient: client => client.DefaultRequestHeaders.Add("X-MyCustomHeader", "value"));
                            break;

                        // case "zipkin":
                        //     options.AddZipkinExporter();
                        //
                        //     builder.Services.Configure<ZipkinExporterOptions>(builder.Configuration.GetSection("Zipkin"));
                        //     break;

                        case "otlp": //applies to both opentelemetry-collector as grafana's tempo 
                            options.AddOtlpExporter("traces", otlpOptions =>
                            {
                                otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                                otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Otlp:Endpoint")! + "/v1/traces"); // 
                                Console.WriteLine(otlpOptions.Endpoint);
                            });
                            break;
                        default:
                            options.AddConsoleExporter();
                            break;
                    }
                    Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
                })
                .WithMetrics(options =>
                {
                    options
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
                            options.AddPrometheusExporter();
                            break;
                        case "otlp":
                            options.AddOtlpExporter("metrics", otlpOptions =>
                            {
                                otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                                otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Otlp:Endpoint")!+ "/v1/metrics");
                            });
                            break;
                        default:
                            options.AddConsoleExporter();
                            break;
                    }
                })
                .StartWithHost();
            return services;
        }

        private static void EnrichWithHttpResponse(Activity activity, HttpResponse response)
        {
            activity.AddTag("http.response_content_length", response.ContentLength);
            activity.AddTag("http.response_content_type", response.ContentType);
        }

        private static void EnrichWithHttpRequest(Activity activity, HttpRequest request)
        {
            var context = request.HttpContext;
            activity.AddTag("http.flavor", GetHttpFlavour(request.Protocol));
            activity.AddTag("http.scheme", request.Scheme);
            activity.AddTag("http.client_ip", context.Connection.RemoteIpAddress);
            activity.AddTag("http.request_content_length", request.ContentLength);
            activity.AddTag("http.request_content_type", request.ContentType);
        }

        private static string GetHttpFlavour(string protocol)
        {
            if (HttpProtocol.IsHttp10(protocol))
            {
                return "1.0";
            }
            if (HttpProtocol.IsHttp11(protocol))
            {
                return "1.1";
            }
            if (HttpProtocol.IsHttp2(protocol))
            {
                return "2.0";
            }
            if (HttpProtocol.IsHttp3(protocol))
            {
                return "3.0";
            }
            throw new InvalidOperationException($"Protocol {protocol} not recognised.");
        }
    }
}
