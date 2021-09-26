using System;
using System.Collections.Generic;
using EmojiUI.Controllers.Dtos;


namespace EmojiUI.Shared.Store
{
    public class VoteState
    {
        public bool IsLoading { get; }
        public IEnumerable<Emoji> Emojies { get; }
        public Emoji SelectedEmoji { get; set; }
        public string Error { get; set; } = string.Empty;

        public VoteState(bool isLoading, IEnumerable<Emoji> emojies, Emoji selectedEmoji = null, string error = "")
        {
            IsLoading = isLoading;
            Emojies = emojies ?? Array.Empty<Emoji>();
            SelectedEmoji = selectedEmoji;
            Error = error;
        }
    }
}
