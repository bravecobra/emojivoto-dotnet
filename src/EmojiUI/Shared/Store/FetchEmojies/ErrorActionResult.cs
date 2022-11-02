using EmojiUI.Controllers.Dtos;


namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class ErrorActionResult
    {
        public string Error { get; set; }
        public Emoji SelectedEmoji { get; set; }

        public ErrorActionResult(Emoji emoji, string error)
        {
            SelectedEmoji = emoji;
            Error = error;
        }
    }
}
