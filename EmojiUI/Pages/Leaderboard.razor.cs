using EmojiUI.Shared.Store.LeaderBoard;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EmojiUI.Pages
{
    public partial class Leaderboard
    {
        [Inject]
        private IState<LeaderBoardState> LeaderBoardState { get; set; }
        [Inject]
        private IDispatcher Dispatcher { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new FetchResultsAction());
        }
    }
}
