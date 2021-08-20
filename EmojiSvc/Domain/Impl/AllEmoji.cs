using System.Collections.Generic;
using System.Linq;
using EmojiSvc.Persistence;

namespace EmojiSvc.Domain.Impl
{
    public class AllEmoji : IAllEmoji
    {
        private readonly IEmojiRepo _repo;

        public AllEmoji(IEmojiRepo repo)
        {
            _repo = repo;
        }

        public Emoji? WithShortcode(string shortcode)
        {
            return _repo.List().FirstOrDefault(e => e.Shortcode == shortcode);
        }

        public List<Emoji> List()
        {
            return _repo.List().ToList();
        }
    }
}