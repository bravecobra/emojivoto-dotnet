using EmojiUI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Emoji = EmojiUI.Controllers.Dtos.Emoji;

namespace EmojiUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmojisController : ControllerBase
    {
        private readonly IEmojiVoteService _voteService;
        private static readonly ActivitySource MyActivitySource = new ActivitySource(nameof(EmojisController));

        public EmojisController(IEmojiVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet]
        public async Task<IEnumerable<Emoji>> ListEmojis()
        {
            using var activity = MyActivitySource.StartActivity(nameof(ListEmojis));
            Activity.Current?.AddEvent(new ActivityEvent("ListEmojies requested"));
            return await _voteService.ListEmojis();
        }

        [HttpGet("{shortcode}")]
        public async Task<Emoji?> FindByShortCode(string shortcode)
        {
            using var activity = MyActivitySource.StartActivity(nameof(FindByShortCode));
            activity?.SetTag("vote.shortcode", shortcode);
            var response = await _voteService.FindByShortCode(shortcode);
            return response != null ? 
                new Emoji { Shortcode = response.Shortcode, Unicode = response.Unicode } : null;
        }
    }
}