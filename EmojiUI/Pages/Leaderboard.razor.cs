using EmojiUI.Shared.Store.LeaderBoard;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EmojiUI.Pages
{
    public partial class Leaderboard
    {
        [Inject]
        private IState<LeaderBoardState> LeaderBoardState { get; set; } = null!;

        [Inject]
        private IDispatcher Dispatcher { get; set; } = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new FetchResultsAction());
        }
    }
}
