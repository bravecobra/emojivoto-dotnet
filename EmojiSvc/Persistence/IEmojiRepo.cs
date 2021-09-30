using System.Collections.Generic;
using System.Threading.Tasks;
using EmojiSvc.Domain;

namespace EmojiSvc.Persistence
{
    public interface IEmojiRepo
    {
        IReadOnlyCollection<Emoji> List();
    }
}