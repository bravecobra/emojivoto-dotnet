using EmojiUI.Controllers.Dtos;
using System.Collections.Generic;

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