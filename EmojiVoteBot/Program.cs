using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmojiVoteBot.Services;
using EmojiVoteBot.Services.Impl;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
                .ConfigureLogging(builder => builder.AddOpenTelemetry())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IEmojiVoteService, EmojiVoteRestService>();
                    services.AddHostedService<VotingBot>();
                    var resourceBuilder = ResourceBuilder.CreateDefault()
                        .AddService(Assembly.GetEntryAssembly()?.GetName().Name)
                        .AddTelemetrySdk();
                    services.AddOpenTelemetryTracing(
                        (builder) => builder
                            .SetSampler(new AlwaysOnSampler())
                            .AddSource(nameof(VotingBot))
                            .AddHttpClientInstrumentation()
                            .AddGrpcClientInstrumentation()
                            .AddConsoleExporter()
                            .AddJaegerExporter(options =>
                            {
                                options.AgentHost = "jaeger";
                                options.AgentPort = 6831;
                            })
                            .SetResourceBuilder(resourceBuilder)
                    );
                });
    }
}