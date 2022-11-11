using EmojiVoting.Domain;
using EmojiVoting.Persistence;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmojiVoting.Application.Impl
{
    public class PollService : IPollService
    {
        private readonly ILogger<PollService> _logger;
        private readonly IVotingRepository _repository;

        public PollService(ILogger<PollService> logger, IVotingRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Vote(string choice)
        {
            var vote = await _repository.GetResultByShortcode(choice);
            if (vote != null)
            {
                vote.Votes++;
                await _repository.UpdateVote(vote);
            }
            else
            {
                vote = new Result { Votes = 1, Shortcode = choice };
                await _repository.AddVote(vote);
            }

            //TODO: Add prometheus custom Counter metric.
            var meter = new Meter(Assembly.GetEntryAssembly()?.GetName().Name ?? "EmojiVoting");
            var counter = meter.CreateCounter<int>("Votes");
            counter.Add(1, KeyValuePair.Create<string, object?>("name", choice));
            _logger.LogInformation("Voted for {choice}, which now has a total of {vote.Votes}", choice, vote.Votes);
        }

        public async Task<List<Result>> Results()
        {
            var list = await _repository.GetResults();
            return list.ToList();
        }
    }
}
