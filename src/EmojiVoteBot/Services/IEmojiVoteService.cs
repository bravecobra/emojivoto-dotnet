using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmojiVoteBot.Services
{
    public interface IEmojiVoteService
    {
        Task<IEnumerable<Emoji>> ListEmojis();
        Task<Emoji?> FindByShortCode(string shortcode);
        Task<bool> Vote(string choice);
    }
}
