using System.ComponentModel.DataAnnotations;

namespace EmojiVoting.Domain
{
    public class Result
    {
        [Key]
        public string Shortcode { get; set; }
        public int Votes { get; set; }
    }
}