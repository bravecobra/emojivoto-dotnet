using EmojiVoteBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmojiShared.Configuration;

namespace EmojiVoteBot;

public class VotingBot : BackgroundService
{
    private readonly ILogger<VotingBot> _logger;
    private readonly IEmojiVoteService _service;
    private readonly IConfiguration _configuration;

    public VotingBot(ILogger<VotingBot> logger, IEmojiVoteService service, IConfiguration configuration)
    {
        _logger = logger;
        _service = service;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var activitySource = ActivitySourceFactory.CreateActivitySource();
            using var activity = activitySource.StartActivity(nameof(VotingBot), ActivityKind.Producer);
            {
                try
                {
                    var probability = Random.Shared.Next(1, 100);
                    if (probability < 15)
                    {
                        activity?.SetTag("voting.shortcode", ":doughnut:");
                        var doughnut = await _service.FindByShortCode(":doughnut:");

                        await _service.Vote(doughnut.Shortcode);
                        _logger.LogInformation($"Forced voting for {doughnut.Unicode}: {doughnut.Shortcode} ");
                    }
                    else
                    {
                        var availableCodes = await _service.ListEmojis();
                        var randomEmoji = availableCodes.ElementAt(Random.Shared.Next(0, 99));
                        activity?.SetTag("voting.shortcode", randomEmoji.Shortcode);
                        await _service.Vote(randomEmoji.Shortcode);
                        _logger.LogInformation($"Voted for {randomEmoji.Unicode}: {randomEmoji.Shortcode}");
                    }

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {

                    _logger.LogError(e, "Failed to vote");
                }
            }
            var votingRate = _configuration.GetValue<int>("VOTING_RATE", 1000);
            await Task.Delay(votingRate, stoppingToken);
        }
    }
}