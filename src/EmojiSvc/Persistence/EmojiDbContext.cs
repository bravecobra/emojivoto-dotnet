using EmojiSvc.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmojiSvc.Persistence
{
    public class EmojiDbContext : DbContext
    {
        public DbSet<Emoji> Emojies { get; set; } = null!;
        public string DbPath { get; private set; }

        public EmojiDbContext(DbContextOptions<EmojiDbContext> options) : base(options)
        {
            DbPath = "emojies.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
