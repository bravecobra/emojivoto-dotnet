using EmojiVoting.Application.Impl;
using EmojiVoting.Domain;
using EmojiVoting.Persistence;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace EmojiVoting.Tests
{
    public class PollServiceShould
    {
        [Fact]
        public async Task CaptureVotesForAnEmoji_GivenEmojiHasBeenVotedFor()
        {
            const string choosenEmoji = ":winning";
            var repoMock = new Mock<IVotingRepository>();
            var aResult = new Result() { Shortcode = choosenEmoji, Votes = 1 };
            repoMock.Setup(repository => repository.AddVote(It.IsAny<Result>()));
            repoMock.Setup(repository => repository.UpdateVote(It.IsAny<Result>()));
            repoMock.Setup(repository => repository.GetResultByShortcode(It.Is<string>(s => s == choosenEmoji))).ReturnsAsync(aResult);
            repoMock.Setup(repository => repository.GetResults()).ReturnsAsync(() => new List<Result> { aResult });
            var sut = new PollService(NullLogger<PollService>.Instance, repoMock.Object);
            await sut.Vote(choosenEmoji);
            await sut.Vote(choosenEmoji);
            var results = await sut.Results();
            repoMock.Verify(repository => repository.AddVote(It.IsAny<Result>()), Times.Never);
            repoMock.Verify(repository => repository.UpdateVote(It.IsAny<Result>()), Times.Exactly(2));
            repoMock.Verify(repository => repository.GetResultByShortcode(It.Is<string>(s => s == choosenEmoji)), Times.Exactly(2));

            Debug.Assert(results != null, nameof(results) + " != null");
            Assert.Equal(3, results.Find(r => r.Shortcode == choosenEmoji)?.Votes);
        }
        [Fact]
        public async Task CaptureVotesForAnEmoji_GivenEmojiHasNotBeenVotedFor()
        {
            var choosenEmoji = ":winning";
            var repoMock = new Mock<IVotingRepository>();
            var aResult = new Result() { Shortcode = choosenEmoji, Votes = 1 };
            repoMock.Setup(repository => repository.AddVote(It.IsAny<Result>()));
            repoMock.Setup(repository => repository.UpdateVote(It.IsAny<Result>()));
            repoMock.Setup(repository => repository.GetResultByShortcode(It.Is<string>(s => s == choosenEmoji))).ReturnsAsync(() => null);
            repoMock.Setup(repository => repository.GetResults()).ReturnsAsync(() => new List<Result> { aResult });
            var sut = new PollService(NullLogger<PollService>.Instance, repoMock.Object);
            await sut.Vote(choosenEmoji);
            var results = await sut.Results();
            repoMock.Verify(repository => repository.AddVote(It.IsAny<Result>()), Times.Once);
            repoMock.Verify(repository => repository.UpdateVote(It.IsAny<Result>()), Times.Never);
            repoMock.Verify(repository => repository.GetResultByShortcode(It.Is<string>(s => s == choosenEmoji)), Times.Exactly(1));

            Debug.Assert(results != null, nameof(results) + " != null");
            Assert.Equal(1, results.Find(r => r.Shortcode == choosenEmoji)?.Votes);
        }
    }
}
