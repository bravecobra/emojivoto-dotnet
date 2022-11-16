using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace EmojiShared.Configuration
{
    public static  class TelemetryConfigurationTracingExtensions
    {
        public static IServiceCollection AddCustomTracing(this IServiceCollection services,
            IConfiguration configuration, ResourceBuilder resourceBuilder, string[] sources)
        {
            services.AddOpenTelemetryTracing(options =>
            {
                options
                    .SetResourceBuilder(resourceBuilder)
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
                        instrumentationOptions.Enrich = Enrich;
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
                        options.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                            otlpOptions.Endpoint = new Uri(configuration.GetValue<string>("Otlp:Endpoint") + "/v1/traces");
                            otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
                        });
                        break;

                    default:
                        options.AddConsoleExporter();

                        break;
                }
                Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
            });
            services.Configure<AspNetCoreInstrumentationOptions>(configuration.GetSection("AspNetCoreInstrumentation"));
            return services;
        }

        private static void Enrich(Activity activity, string eventName, object obj)
        {
            switch (obj)
            {
                case HttpRequest request:
                    {
                        var context = request.HttpContext;
                        activity.AddTag("http.flavor", GetHttpFlavour(request.Protocol));
                        activity.AddTag("http.scheme", request.Scheme);
                        activity.AddTag("http.client_ip", context.Connection.RemoteIpAddress);
                        activity.AddTag("http.request_content_length", request.ContentLength);
                        activity.AddTag("http.request_content_type", request.ContentType);
                        break;
                    }
                case HttpResponse response:
                    activity.AddTag("http.response_content_length", response.ContentLength);
                    activity.AddTag("http.response_content_type", response.ContentType);
                    break;
            }
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
