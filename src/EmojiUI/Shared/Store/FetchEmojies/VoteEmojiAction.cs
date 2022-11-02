using EmojiUI.Controllers.Dtos;


namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class VoteEmojiAction
    {
        public Emoji Choice { get; set; }

        public VoteEmojiAction(Emoji emoji)
        {
            Choice = emoji;
        }
    }
}
