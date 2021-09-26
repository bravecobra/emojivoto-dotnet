using System.Collections.Generic;
using EmojiUI.Controllers.Dtos;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class FetchEmojiesResultAction
    {
        public IEnumerable<Emoji> Emojies { get; }

        public FetchEmojiesResultAction(IEnumerable<Emoji> emojies)
        {
            Emojies = emojies;
        }
    }
}