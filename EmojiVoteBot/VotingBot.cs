using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmojiVoteBot.Services;

namespace EmojiVoteBot
{
    public class VotingBot : BackgroundService
    {
        private readonly ILogger<VotingBot> _logger;
        private readonly IEmojiVoteService _service;

        public VotingBot(ILogger<VotingBot> logger, IEmojiVoteService service)
        {
            _logger = logger;
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var random = new Random();
                    var probability = random.Next(1, 100);
                    if (probability < 15)
                    {
                        var doughnut = await _service.FindByShortCode(":doughnut:");
                        await _service.Vote(doughnut.Shortcode);
                        _logger.LogInformation($"Forced voting for {doughnut.Unicode}: {doughnut.Shortcode} ");
                    }
                    else
                    {
                        var availableCodes = await _service.ListEmojis();
                        var randomEmoji = availableCodes.ElementAt(random.Next(0, 99));
                        await _service.Vote(randomEmoji.Shortcode);
                        _logger.LogInformation($"Voted for {randomEmoji.Unicode}: {randomEmoji.Shortcode}");
                    }

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to vote");
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
