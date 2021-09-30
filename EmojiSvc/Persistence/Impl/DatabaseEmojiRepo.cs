﻿using System.Collections.Generic;
using System.Linq;
using EmojiSvc.Domain;

namespace EmojiSvc.Persistence.Impl
{
    public class DatabaseEmojiRepo: IEmojiRepo
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
