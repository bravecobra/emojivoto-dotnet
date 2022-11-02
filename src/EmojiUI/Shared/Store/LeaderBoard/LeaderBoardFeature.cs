using Fluxor;

namespace EmojiUI.Shared.Store.LeaderBoard
{
    public class LeaderBoardFeature : Feature<LeaderBoardState>
    {
        public override string GetName() => "LeaderBoard";

        protected override LeaderBoardState GetInitialState()
        {
            return new LeaderBoardState(
                isLoading: true,
                results: null,
                error: string.Empty);
        }
    }
}