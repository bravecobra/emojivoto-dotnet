using EmojiVoteBot.Services;
using EmojiVoteBot.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using EmojiShared.Configuration;

namespace EmojiVoteBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            Console.OutputEncoding = Encoding.UTF8;
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var resourceBuilder = ResourceBuilderFactory.CreateResourceBuilder();
            return Host.CreateDefaultBuilder(args)
                .AddCustomLogging(resourceBuilder)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOpenTelemetryServices();
                    services.AddCustomMetrics(hostContext.Configuration, resourceBuilder);
                    services.AddCustomTracing(hostContext.Configuration, resourceBuilder, new []{ nameof(VotingBot)});

                    services.AddHttpClient();
                    services.AddTransient<IEmojiVoteService, EmojiVoteRestService>();
                    services.AddHostedService<VotingBot>();
                });
        }
    }
}
