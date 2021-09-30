using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EmojiUI.Controllers.Dtos;
using EmojiUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmojiUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IEmojiVoteService _voteService;

        public VoteController(IEmojiVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet]
        public async Task<IEnumerable<Result>> GetResults()
        {
            using var activity = Activity.Current?.Source.StartActivity(nameof(GetResults));
            Activity.Current?.AddEvent(new ActivityEvent("Results requested"));
            return await _voteService.GetResults();
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromQuery] string choice)
        {
            using var activity = Activity.Current?.Source.StartActivity(nameof(Vote));
            activity?.SetTag("vote.choice", choice);
            if (await _voteService.Vote(choice))
                return Accepted();
            return BadRequest();
        }
    }
}