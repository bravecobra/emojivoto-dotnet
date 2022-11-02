using EmojiUI.Controllers.Dtos;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class VoteEmojiActionResult
    {
        public Emoji SelectedEmoji { get; }

        public VoteEmojiActionResult(Emoji emoji)
        {
            SelectedEmoji = emoji;
        }

    }
}
