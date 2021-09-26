using EmojiUI.Shared.Store;
using EmojiUI.Shared.Store.FetchEmojies;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EmojiUI.Pages
{
    public partial class Index: Fluxor.Blazor.Web.Components.FluxorComponent
    {
        [Inject]
        private IState<VoteState> VoteState { get; set; }
        [Inject]
        private IDispatcher Dispatcher { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new FetchEmojiesAction());
        }
    }
}
