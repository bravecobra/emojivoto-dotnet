using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmojiVoteBot.Services;
using EmojiVoteBot.Services.Impl;
using System;
using System.Text;

namespace EmojiVoteBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IEmojiVoteService, EmojiVoteRestService>();
                    services.AddHostedService<VotingBot>();
                });
    }
}
