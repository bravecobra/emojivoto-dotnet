using System;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class FetchResultsErrorAction
    {
        public string Error { get; set; }
        public FetchResultsErrorAction(string message)
        {
            Error = message;
        }
    }
}