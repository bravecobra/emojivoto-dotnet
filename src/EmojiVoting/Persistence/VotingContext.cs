using EmojiVoting.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmojiVoting.Persistence
{
    public class VotingContext : DbContext
    {
        public DbSet<Result> Results { get; set; } = null!;
        public string DbPath { get; private set; }

        public VotingContext(DbContextOptions<VotingContext> options) : base(options)
        {
            DbPath = "voting.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
