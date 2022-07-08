using EmojiSvc.Domain;
using System.Collections.Generic;

namespace EmojiSvc.Persistence.Impl
{
    public class InMemoryAllEmoji : IEmojiRepo
    {
        private readonly List<Emoji> _emojis = new();

        public InMemoryAllEmoji()
        {
            foreach (var s in EmojiDefinitions.Top100Emojis)
            {
                _emojis.Add(new Emoji
                {
                    Shortcode = s,
                    Unicode = EmojiDefinitions.CodeMap[s]
                });
            }
        }

        public IReadOnlyCollection<Emoji> List()
        {
            return _emojis.AsReadOnly();
        }
    }
}
