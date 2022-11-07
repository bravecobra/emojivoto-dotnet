using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace EmojiShared.Configuration
{
    public static class TelemetryConfigurationLoggingExtensions
    {
        public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder, ResourceBuilder resourceBuilder)
        {
            builder.Logging.ClearProviders();
            var logExporter = builder.Configuration.GetValue<string>("UseLogExporter").ToLowerInvariant();
            switch (logExporter)
            {
                case "loki":
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        //.MinimumLevel.Debug()
                        //.WriteTo.Console()
                        // .WriteTo.GrafanaLoki(builder.Configuration.GetValue<string>("Loki:Endpoint"),
                        //     textFormatter: new LokiJsonTextFormatter(),
                        // )
                        .Enrich.FromLogContext()
                        .Enrich.With(new PropertyEnricher("app", Assembly.GetEntryAssembly()?.GetName().Name!))
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithAssemblyInformationalVersion()
                        .Enrich.WithAssemblyName()
                        .Enrich.WithAssemblyVersion(true)
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithEnvironmentUserName()
                        //.Enrich.WithMachineName()
                        .Enrich.WithProcessId()
                        .Enrich.WithProcessName()
                        .Enrich.WithMemoryUsage()
                        .Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .Enrich.WithSpan(new SpanOptions
                        {
                            IncludeBaggage = true,
                            IncludeTags = true,
                            LogEventPropertiesNames = new SpanLogEventPropertiesNames
                            {
                                TraceId = "traceid"
                            }
                        }) //renamed to let it match with derived fields for OTEL
                        .CreateLogger();
                    builder.Logging.AddSerilog(logger);
                    break;
                }
                case "seq":
                {
                    var seq_uri = builder.Configuration.GetValue<string>("Seq:Uri").ToLowerInvariant();
                    var logger = new LoggerConfiguration()
                        .WriteTo.Seq(seq_uri)
                        .Enrich.FromLogContext()
                        .Enrich.With(new PropertyEnricher("app", Assembly.GetEntryAssembly()?.GetName().Name!))
                        .Enrich.WithExceptionDetails()
                        .Enrich.WithAssemblyInformationalVersion()
                        .Enrich.WithAssemblyName()
                        .Enrich.WithAssemblyVersion(true)
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithEnvironmentUserName()
                        //.Enrich.WithMachineName()
                        .Enrich.WithProcessId()
                        .Enrich.WithProcessName()
                        .Enrich.WithMemoryUsage()
                        .Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .Enrich.WithSpan(new SpanOptions
                            { LogEventPropertiesNames = new SpanLogEventPropertiesNames
                            {
                                TraceId = "traceid"
                            } })
                        .CreateLogger();
                    builder.Logging.AddSerilog(logger);
                    break;
                }
                case "otlp":
                {
                    builder.Logging.AddOpenTelemetry(options =>
                    {
                        options.SetResourceBuilder(resourceBuilder);
                        options.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                            otlpOptions.Endpoint =
                                new Uri(builder.Configuration.GetValue<string>("Otlp:Endpoint") + "/v1/logs");
                            otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
                        });
                        options.IncludeFormattedMessage = true;
                        options.ParseStateValues = true;
                        options.IncludeScopes = true;
                    });
                    builder.Services.AddOpenTelemetryServices();
                    break;
                }
                default:
                    builder.Logging.AddSerilog();
                    break;
            }
            return builder;
        }

        public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services)
        {
            services.Configure<OpenTelemetryLoggerOptions>(opt =>
            {
                opt.IncludeScopes = true;
                opt.ParseStateValues = true;
                opt.IncludeFormattedMessage = true;
            });
            return services;
        }

        public static IHostBuilder AddCustomLogging(this IHostBuilder builder, ResourceBuilder resourceBuilder)
        {
            builder.ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                var logExporter = context.Configuration.GetValue<string>("UseLogExporter").ToLowerInvariant();
                switch (logExporter)
                {
                    case "loki":
                        {
                            var logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(context.Configuration)
                                //.MinimumLevel.Debug()
                                //.WriteTo.Console()
                                // .WriteTo.GrafanaLoki(builder.Configuration.GetValue<string>("Loki:Endpoint"),
                                //     textFormatter: new LokiJsonTextFormatter(),
                                // )
                                .Enrich.FromLogContext()
                                .Enrich.With(new PropertyEnricher("app", Assembly.GetEntryAssembly()?.GetName().Name!))
                                .Enrich.WithExceptionDetails()
                                .Enrich.WithAssemblyInformationalVersion()
                                .Enrich.WithAssemblyName()
                                .Enrich.WithAssemblyVersion(true)
                                .Enrich.WithEnvironmentName()
                                .Enrich.WithEnvironmentUserName()
                                //.Enrich.WithMachineName()
                                .Enrich.WithProcessId()
                                .Enrich.WithProcessName()
                                .Enrich.WithMemoryUsage()
                                .Enrich.WithThreadId()
                                .Enrich.WithThreadName()
                                .Enrich.WithSpan(new SpanOptions
                                {
                                    IncludeBaggage = true,
                                    IncludeTags = true,
                                    LogEventPropertiesNames = new SpanLogEventPropertiesNames
                                    {
                                        TraceId = "traceid"
                                    }
                                }) //renamed to let it match with derived fields for OTEL
                                .CreateLogger();
                            loggingBuilder.AddSerilog(logger);
                            break;
                        }
                    case "seq":
                        {
                            var seq_uri = context.Configuration.GetValue<string>("Seq:Uri").ToLowerInvariant();
                            var logger = new LoggerConfiguration()
                                .WriteTo.Seq(seq_uri)
                                .Enrich.FromLogContext()
                                .Enrich.With(new PropertyEnricher("app", Assembly.GetEntryAssembly()?.GetName().Name!))
                                .Enrich.WithExceptionDetails()
                                .Enrich.WithAssemblyInformationalVersion()
                                .Enrich.WithAssemblyName()
                                .Enrich.WithAssemblyVersion(true)
                                .Enrich.WithEnvironmentName()
                                .Enrich.WithEnvironmentUserName()
                                //.Enrich.WithMachineName()
                                .Enrich.WithProcessId()
                                .Enrich.WithProcessName()
                                .Enrich.WithMemoryUsage()
                                .Enrich.WithThreadId()
                                .Enrich.WithThreadName()
                                .Enrich.WithSpan(new SpanOptions
                                {
                                    LogEventPropertiesNames = new SpanLogEventPropertiesNames
                                    {
                                        TraceId = "traceid"
                                    }
                                })
                                .CreateLogger();
                            loggingBuilder.AddSerilog(logger);
                            break;
                        }
                    case "otlp":
                        {
                            loggingBuilder.AddOpenTelemetry(options =>
                            {
                                options.SetResourceBuilder(resourceBuilder);
                                options.AddOtlpExporter(otlpOptions =>
                                {
                                    otlpOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                                    otlpOptions.Endpoint =
                                        new Uri(context.Configuration.GetValue<string>("Otlp:Endpoint") + "/v1/logs");
                                    otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
                                });
                            });

                            break;
                        }
                    default:
                        loggingBuilder.AddOpenTelemetry(options =>
                        {
                            options.SetResourceBuilder(resourceBuilder);
                            options.AddConsoleExporter();
                        });
                        break;
                }
            });
            return builder;
        }


    }
}
