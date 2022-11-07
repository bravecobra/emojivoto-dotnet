using EmojiVoting.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

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
