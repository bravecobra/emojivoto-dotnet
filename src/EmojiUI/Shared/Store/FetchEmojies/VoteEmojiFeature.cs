using Fluxor;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class VoteEmojiFeature : Feature<VoteState>
    {
        public override string GetName() => "FetchEmojies";

        protected override VoteState GetInitialState()
        {
            return new VoteState(
                isLoading: false,
                emojies: null);
        }
    }
}
