using EmojiVoting.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmojiVoting.Persistence.Impl
{
    public class VotingRepository : IVotingRepository
    {
        private readonly VotingContext _context;

        public VotingRepository(VotingContext context)
        {
            _context = context;
        }
        public async Task<Result> GetResultByShortcode(string shortcode)
        {
            return await _context.Results.Where(result => result.Shortcode == shortcode).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateVote(Result result)
        {
            return (await _context.SaveChangesAsync() != 0);
        }

        public async Task<bool> AddVote(Result result)
        {
            await _context.AddAsync(result);
            return (await _context.SaveChangesAsync() != 0);
        }

        public async Task<IEnumerable<Result>> GetResults()
        {
            return (await _context.Results.AsNoTracking().ToListAsync(CancellationToken.None)).OrderByDescending(result => result.Votes).AsEnumerable();
        }
    }
}