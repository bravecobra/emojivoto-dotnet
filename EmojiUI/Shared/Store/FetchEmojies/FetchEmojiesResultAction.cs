using EmojiUI.Controllers.Dtos;
using System.Collections.Generic;

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