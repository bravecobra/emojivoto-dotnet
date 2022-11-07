using EmojiSvc.Domain.Impl;
using EmojiSvc.Persistence.Impl;
using Xunit;

namespace EmojiSvc.Tests.Domain.Impl
{
    public class AllEmojiShould
    {
        [Fact]
        public void FindAnEmojiByShortcode()
        {
            var sut = new AllEmoji(new InMemoryAllEmoji());

            foreach (var emoji in sut.List())
            {
                Assert.NotNull(sut.WithShortcode(emoji.Shortcode));
            }
        }

        [Fact]
        public void ReturnNullWhenEmojiFotFoundByShortCode()
        {
            var sut = new AllEmoji(new InMemoryAllEmoji());
            var nonexisting = new[] { "these", "arent", "emoji", "shortcodes" };
            foreach (var s in nonexisting)
            {
                Assert.Null(sut.WithShortcode(s));
            }
        }
    }
}
