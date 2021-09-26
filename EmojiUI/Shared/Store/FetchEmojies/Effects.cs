using System;
using System.Threading.Tasks;
using EmojiUI.Services;
using Fluxor;

namespace EmojiUI.Shared.Store.FetchEmojies
{
    public class Effects
    {
        private readonly IEmojiVoteService _service;

        public Effects(IEmojiVoteService service)
        {
            _service = service;
        }

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchEmojiesAction action, IDispatcher dispatcher)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            var result = await _service.ListEmojis();
            dispatcher.Dispatch(new FetchEmojiesResultAction(result));
        }

        [EffectMethod]
        public async Task HandleVoteEmojiAction(VoteEmojiAction action, IDispatcher dispatcher)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            try
            {
                var result = await _service.Vote(action.Choice.Shortcode);
                if (result)
                {
                    dispatcher.Dispatch(new VoteEmojiActionResult(action.Choice));
                }
            }
            catch (Exception)
            {
                dispatcher.Dispatch(new ErrorActionResult(action.Choice, "Unable to Register Vote"));
            }
        }
    }
}
