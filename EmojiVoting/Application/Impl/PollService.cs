using EmojiVoting.Domain;
using EmojiVoting.Persistence;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmojiVoting.Application.Impl
{
    public class PollService : IPollService
    {
        private readonly ILogger<PollService> _logger;
        private readonly IVotingRepository _repository;
        //private readonly ConcurrentDictionary<string, Result> _votes = new(); //TODO: Move to persistence layer
        private int _voteCounter = 0;

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
            _voteCounter++;
            //TODO: Add prometheus custom Counter metric.
            _logger.LogInformation($"Voted for {choice}, which now has a total of {vote.Votes}");
        }

        public async Task<List<Result>> Results()
        {
            var list = await _repository.GetResults();
            return list.ToList();
        }
    }
}
