using EmojiUI.Controllers.Dtos;
using System.Collections.Generic;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class LeaderBoardState
    {
        public bool IsLoading { get; set; }
        public IEnumerable<Result>? Results { get; set; }
        public string Error { get; set; }

        public LeaderBoardState(bool isLoading, IEnumerable<Result>? results, string error = "")
        {
            IsLoading = isLoading;
            Results = results;
            Error = error;
        }
    }
}
