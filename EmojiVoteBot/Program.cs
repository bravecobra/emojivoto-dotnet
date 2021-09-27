using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmojiVoteBot.Services;
using EmojiVoteBot.Services.Impl;
using System;
using System.Diagnostics;
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
                    var resourceBuilder = ResourceBuilder.CreateDefault().AddService("EmojiVoteBot");
                    services.AddOpenTelemetryTracing(
                        (builder) => builder
                            .AddHttpClientInstrumentation()
                            .AddGrpcClientInstrumentation()
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
