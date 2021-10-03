using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmojiVoteBot.Services;
using EmojiVoteBot.Services.Impl;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace EmojiVoteBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            Console.OutputEncoding = Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(builder => builder.AddOpenTelemetry())
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithAssemblyName()
                        .Enrich.WithAssemblyVersion()
                        .Enrich.WithAssemblyInformationalVersion()
                        .Enrich.WithEnvironment(context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProcessId()
                        .Enrich.WithProcessName()
                        .Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .Enrich.WithSpan()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console(new RenderedCompactJsonFormatter());
                    if (!string.IsNullOrEmpty(context.Configuration["SEQ_URI"]))
                    {
                        loggerConfiguration.WriteTo.Seq(context.Configuration["SEQ_URI"]);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IEmojiVoteService, EmojiVoteRestService>();
                    services.AddHostedService<VotingBot>();
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                    var resourceBuilder = ResourceBuilder.CreateDefault()
                        .AddService(Assembly.GetEntryAssembly()?.GetName().Name)
                        .AddTelemetrySdk();
                    services.AddOpenTelemetryTracing(
                        (builder) =>
                        {
                            builder
                                .SetSampler(new AlwaysOnSampler())
                                .AddSource(nameof(VotingBot))
                                .AddXRayTraceId()
                                .AddAWSInstrumentation()
                                .AddHttpClientInstrumentation(options => options.RecordException = false)
                                .AddGrpcClientInstrumentation()
                                .SetResourceBuilder(resourceBuilder);
                            var consoleExport = hostContext.Configuration.GetValue<bool>("CONSOLE_EXPORT");
                            if (consoleExport)
                            {
                                builder.AddConsoleExporter();
                            }
                            var jaegerHost = hostContext.Configuration.GetValue<string>("JAEGER_HOST");
                            var jaegerPort = hostContext.Configuration.GetValue<int>("JAEGER_PORT");
                            if (!string.IsNullOrEmpty(jaegerHost))
                            {
                                builder.AddJaegerExporter(options =>
                                {
                                    options.AgentHost = jaegerHost;
                                    options.AgentPort = jaegerPort;
                                });
                            }
                            var otelUri = hostContext.Configuration["AWS_OTEL_URI"];
                            if (!string.IsNullOrEmpty(otelUri))
                            {
                                builder.AddOtlpExporter(options =>
                                {
                                    options.Endpoint = new Uri(otelUri);
                                });
                            }
                        });
                    Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
                });
    }
}
