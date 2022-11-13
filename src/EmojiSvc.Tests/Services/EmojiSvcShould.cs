using AutoMapper;
using EmojiSvc.Domain;
using EmojiSvc.Domain.Impl;
using EmojiSvc.Persistence.Impl;
using EmojiSvc.Services;
using Emojivoto.V1;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace EmojiSvc.Tests.Services
{
    public class EmojiSvcShould
    {
        private readonly IMapper _mapper;
        public EmojiSvcShould()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EmojiProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task ReturnAllEmojis()
        {
            var sut = new EmojiGrpcSvc(NullLogger<EmojiGrpcSvc>.Instance, new AllEmoji(new InMemoryAllEmoji()), _mapper);
            var actual = await sut.ListAll(new ListAllEmojiRequest(), null!);
            Assert.Equal(99, actual.List.Count);
        }

        [Fact]
        public async Task FindAnEmojiThroughItsShortcodeGivenItExists()
        {
            var allEmoji = new AllEmoji(new InMemoryAllEmoji());
            var emojiSearchedFor = allEmoji.List()[3];
            var sut = new EmojiGrpcSvc(NullLogger<EmojiGrpcSvc>.Instance, allEmoji, _mapper);
            var actual = await sut.FindByShortcode(new FindByShortcodeRequest() { Shortcode = emojiSearchedFor.Shortcode }, null!);
            Assert.NotNull(actual.Emoji);
        }

        [Fact]
        public async Task NotFindAnEmojiThroughItsShortcodeGivenItDoesntExist()
        {
            var allEmoji = new AllEmoji(new InMemoryAllEmoji());
            var sut = new EmojiGrpcSvc(NullLogger<EmojiGrpcSvc>.Instance, allEmoji, _mapper);
            var actual = await sut.FindByShortcode(new FindByShortcodeRequest() { Shortcode = "notexists" }, null!);
            Assert.Null(actual.Emoji);
        }
    }
}
