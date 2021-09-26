using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmojiUI.Shared.Store.FetchEmojies;
using Fluxor;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class Reducers
    {
        [ReducerMethod]
        public static LeaderBoardState ReduceFetchEmojiesAction(LeaderBoardState state, FetchResultsAction action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new LeaderBoardState(
                isLoading: true,
                results: null,
                error: String.Empty);
        }

        [ReducerMethod]
        public static LeaderBoardState ReduceFetchEmojiesResultAction(LeaderBoardState state, FetchResultsActionResult action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new LeaderBoardState(
                isLoading: false,
                results: action.Results,
                error: String.Empty);
        }

        [ReducerMethod]
        public static LeaderBoardState ReduceErrorResultAction(LeaderBoardState state, FetchResultsErrorAction action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new LeaderBoardState(
                isLoading: false,
                results: null,
                error: action.Error);
        }
    }
}
