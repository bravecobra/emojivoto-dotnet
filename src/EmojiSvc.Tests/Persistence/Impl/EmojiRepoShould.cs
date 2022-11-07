using EmojiSvc.Persistence.Impl;
using System.Linq;
using Xunit;

namespace EmojiSvc.Tests.Persistence.Impl
{
    public class EmojiRepoShould
    {
        [Fact]
        public void ContainEveryEmojiOfTheTop100()
        {
            var sut = new InMemoryAllEmoji();
            foreach (var s in EmojiDefinitions.Top100Emojis)
            {
                Assert.Single(sut.List().Where(emoji => emoji.Shortcode == s));
            }
        }
    }
}