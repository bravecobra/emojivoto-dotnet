using System.Collections.Generic;
using EmojiSvc.Domain;

namespace EmojiSvc.Persistence
{
    public interface IEmojiRepo
    {
        IReadOnlyCollection<Emoji> List();
    }
}