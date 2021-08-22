using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EmojiVoting.Domain
{
    public class PollService: IPollService
    {
        private readonly ILogger<PollService> _logger;
        private readonly ConcurrentDictionary<string, Result> _votes = new(); //Move to persistence layer
        private int _voteCounter = 0;

        public PollService(ILogger<PollService> logger)
        {
            _logger = logger;
        }

        public void Vote(string choice)
        {
            Result newResult;
            if (_votes.TryGetValue(choice, out var vote))
            {
                newResult = new Result { Shortcode = choice, Votes = ++vote.Votes };
                _votes.TryUpdate(choice, newResult, vote);
            }
            else
            {
                newResult = new Result { Votes = 1, Shortcode = choice };
                _votes.TryAdd(choice, newResult);
            }

            _voteCounter++;
            //TODO: Add prometheus custom Counter metric.
            _logger.LogInformation($"Voted for {choice}, which now has a total of {newResult.Votes}");
        }

        public List<Result> Results()
        {
            return _votes.Values.OrderByDescending(result => result.Votes).ToList();
        }
    }
}
