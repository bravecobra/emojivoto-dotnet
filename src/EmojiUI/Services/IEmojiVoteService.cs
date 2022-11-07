using EmojiUI.Controllers.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmojiUI.Services
{
    public interface IEmojiVoteService
    {
        Task<IEnumerable<Result>> GetResults();
        Task<IEnumerable<Emoji>> ListEmojis();
        Task<Emoji?> FindByShortCode(string shortcode);
        Task<bool> Vote(string choice);
    }
}
