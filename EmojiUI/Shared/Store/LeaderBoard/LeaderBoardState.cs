using System.Collections.Generic;
using EmojiUI.Controllers.Dtos;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class LeaderBoardState
    {
        public bool IsLoading { get; set; }
        public IEnumerable<Result> Results { get; set; }
        public string Error { get; set; } = string.Empty;

        public LeaderBoardState(bool isLoading, IEnumerable<Result> results, string error = "")
        {
            IsLoading = isLoading;
            Results = results;
            Error = error;
        }
    }
}
