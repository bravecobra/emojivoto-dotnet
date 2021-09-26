using System;
using Fluxor;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class Reducers
    {
        [ReducerMethod]
        public static VoteState ReduceFetchEmojiesAction(VoteState state, FetchEmojiesAction action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new VoteState(
                isLoading: true,
                emojies: null);
        }

        [ReducerMethod]
        public static VoteState ReduceFetchEmojiesResultAction(VoteState state, FetchEmojiesResultAction action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new VoteState(
                isLoading: false,
                emojies: action.Emojies,
                selectedEmoji: null,
                error: string.Empty);
        }

        [ReducerMethod]
        public static VoteState ReduceVoteEmojiResultAction(VoteState state, VoteEmojiActionResult action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new VoteState(
                isLoading: false,
                emojies: state.Emojies,
                selectedEmoji: action.SelectedEmoji,
                error: string.Empty);

        }

        [ReducerMethod]
        public static VoteState ReduceErrorResultAction(VoteState state, ErrorActionResult action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new VoteState(
                isLoading: false,
                emojies: state.Emojies,
                selectedEmoji: action.SelectedEmoji,
                error: action.Error);
        }

        [ReducerMethod]
        public static VoteState ReduceResetAction(VoteState state, ResetAction action)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (action == null) throw new ArgumentNullException(nameof(action));
            return new VoteState(
                isLoading: false,
                emojies: state.Emojies,
                selectedEmoji: null,
                error: string.Empty);
        }
    }
}
