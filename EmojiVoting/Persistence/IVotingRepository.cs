using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmojiVoting.Domain;

namespace EmojiVoting.Persistence
{
    public interface IVotingRepository
    {
        Task<Result?> GetResultByShortcode(string shortcode);
        Task<bool> AddVote(Result result);
        Task<bool> UpdateVote(Result result);
        Task<IEnumerable<Result>> GetResults();
    }
}
