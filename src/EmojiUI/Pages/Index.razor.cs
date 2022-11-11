using EmojiUI.Shared.Store.FetchEmojies;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EmojiUI.Pages
{
    public partial class Index : Fluxor.Blazor.Web.Components.FluxorComponent
    {
        [Inject]
        private IState<VoteState> VoteState { get; set; } = null!;

        [Inject]
        private IDispatcher Dispatcher { get; set; } = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new FetchEmojiesAction());
        }
    }
}
