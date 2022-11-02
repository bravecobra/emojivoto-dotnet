namespace EmojiUI.Controllers.Dtos
{
    public class Result
    {
        public string Shortcode { get; init; } = null!;
        public string Unicode { get; init; } = null!;
        public int Votes { get; init; }
    }
}
