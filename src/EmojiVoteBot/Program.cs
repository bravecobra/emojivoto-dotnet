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

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .AddCustomLogging()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOpenTelemetryServices();
                    services.AddCustomTelemetry(hostContext.Configuration, new []{ nameof(VotingBot)});

                    services.AddHttpClient();
                    services.AddTransient<IEmojiVoteService, EmojiVoteRestService>();
                    services.AddHostedService<VotingBot>();
                });
        }
    }
}
