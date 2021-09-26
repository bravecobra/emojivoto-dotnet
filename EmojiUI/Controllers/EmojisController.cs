using System.Collections.Generic;
using System.Threading.Tasks;
using EmojiUI.Services;
using Emojivoto.V1;
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

        [HttpGet("{shortcode}")]
        public async Task<Emoji> FindByShortCode(string shortcode)
        {
            var response = await _voteService.FindByShortCode(shortcode);
            return response != null ? 
                new Emoji { Shortcode = response.Shortcode, Unicode = response.Unicode } : 
                null;
        }
    }
}