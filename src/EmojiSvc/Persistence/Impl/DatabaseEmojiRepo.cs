using EmojiSvc.Domain;
using System.Collections.Generic;
using System.Linq;

namespace EmojiSvc.Persistence.Impl
{
    public class DatabaseEmojiRepo : IEmojiRepo
    {
        private readonly EmojiDbContext _dbContext;

        public DatabaseEmojiRepo(EmojiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IReadOnlyCollection<Emoji> List()
        {
            return _dbContext.Emojies.ToList().AsReadOnly();
        }
    }
}
