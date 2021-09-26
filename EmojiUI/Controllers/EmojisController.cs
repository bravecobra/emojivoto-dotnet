using System.Collections.Generic;
using System.Threading.Tasks;
using EmojiUI.Services;
using Microsoft.AspNetCore.Mvc;
using Emoji = EmojiUI.Controllers.Dtos.Emoji;

namespace EmojiUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmojisController : ControllerBase
    {
        private readonly IEmojiVoteService _voteService;

        public EmojisController(IEmojiVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet]
        public async Task<IEnumerable<Emoji>> ListEmojis()
        {
            return await _voteService.ListEmojis();
        }

        // [HttpGet("{shortcode}")]
        // public async Task<Emoji> FindByShortCode(string shortcode)
        // {
        //     var response = await _client.FindByShortcodeAsync(new FindByShortcodeRequest()
        //     {
        //         Shortcode = shortcode
        //     });
        //     return response.Emoji != null ? 
        //         new Emoji { Shortcode = response.Emoji.Shortcode, Unicode = response.Emoji.Unicode } : 
        //         null;
        // }
    }
}