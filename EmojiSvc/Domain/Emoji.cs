using System.ComponentModel.DataAnnotations;

namespace EmojiSvc.Domain
{
    public class Emoji
    {
        public string Unicode { get; set; } = null!;
        [Key]
        public string Shortcode { get; set; } = null!;
    }
}
