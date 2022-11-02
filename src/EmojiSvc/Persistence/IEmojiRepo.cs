using EmojiSvc.Domain;
using System.Collections.Generic;

namespace EmojiSvc.Persistence
{
    public interface IEmojiRepo
    {
        IReadOnlyCollection<Emoji> List();
    }
}