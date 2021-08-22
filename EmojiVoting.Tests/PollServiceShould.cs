using System;
using System.Diagnostics;
using EmojiVoting.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EmojiVoting.Tests
{
    public class PollServiceShould
    {
        [Fact]
        public void CaptureVotesForAnEmoji()
        {
            var choosenEmoji = ":winning";
            var sut = new PollService(NullLogger<PollService>.Instance);
            sut.Vote(choosenEmoji);
            sut.Vote(choosenEmoji);
            var results = sut.Results();
            Debug.Assert(results != null, nameof(results) + " != null");
            Assert.Equal(2, results.Find(r => r.Shortcode == choosenEmoji).Votes);
        }

        [Fact]
        public void SortResultByDescendingVotesCounts()
        {
            var firstPlace = ":1:";
            var secondPlace = ":2:";
            var thirdPlace = ":3:";
            var sut = new PollService(NullLogger<PollService>.Instance);
            sut.Vote(thirdPlace);
            sut.Vote(firstPlace);
            sut.Vote(secondPlace);
            sut.Vote(firstPlace);
            sut.Vote(secondPlace);
            sut.Vote(firstPlace);
            var results = sut.Results();
            Assert.Equal(3, results.Count);
            Assert.Equal(firstPlace, results[0].Shortcode);
            Assert.Equal(secondPlace, results[1].Shortcode);
            Assert.Equal(thirdPlace, results[2].Shortcode);
        }
    }
}
