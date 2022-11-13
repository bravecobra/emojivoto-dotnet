using EmojiUI.Services;
using Fluxor;
using System;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace EmojiUI.Shared.Store.LeaderBoard
{
    // ReSharper disable once UnusedType.Global
    public class Effects
    {
        private readonly IEmojiVoteService _service;

        public Effects(IEmojiVoteService service)
        {
            _service = service;
        }

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchResultsAction action, IDispatcher dispatcher)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            try
            {
                var result = await _service.GetResults();
                dispatcher.Dispatch(new FetchResultsActionResult(result));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new FetchResultsErrorAction(exception.Message));
            }
        }
    }
}
