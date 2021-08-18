using System.Collections.Generic;

namespace EmojiSvc.Domain
{
    public interface IEmojiRepo
    {
        IReadOnlyCollection<Emoji> List();
    }
}