using System.Collections.Generic;
using EmojiUI.Controllers.Dtos;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class FetchResultsActionResult
    {
        public IEnumerable<Result> Results { get; set; }

        public FetchResultsActionResult(IEnumerable<Result> results)
        {
            Results = results;
        }
    }
}