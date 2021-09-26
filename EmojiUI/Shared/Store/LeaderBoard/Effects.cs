using System;
using System.Threading.Tasks;
using EmojiUI.Services;
using EmojiUI.Services.Impl;
using Fluxor;

namespace EmojiUI.Shared.Store.LeaderBoard
{
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
