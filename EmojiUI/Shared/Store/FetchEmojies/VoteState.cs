using System.Collections.Generic;
using EmojiUI.Controllers.Dtos;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class VoteState
    {
        public bool IsLoading { get; }
        public IEnumerable<Emoji>? Emojies { get; }
        public Emoji? SelectedEmoji { get; set; }
        public string Error { get; set; }

        public VoteState(bool isLoading, IEnumerable<Emoji>? emojies, Emoji? selectedEmoji = null, string error = "")
        {
            IsLoading = isLoading;
            Emojies = emojies;
            SelectedEmoji = selectedEmoji;
            Error = error;
        }
    }
}
