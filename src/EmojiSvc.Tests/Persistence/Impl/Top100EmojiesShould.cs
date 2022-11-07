using EmojiSvc.Persistence.Impl;
using Xunit;

namespace EmojiSvc.Tests.Persistence.Impl
{
    public class Top100EmojiesShould
    {
        [Fact]
        public void ContainAllEmojisOfTheCodeMap()
        {
            var unicodes = EmojiDefinitions.CodeMap.Values;
            foreach (var emoji in EmojiDefinitions.Top100Emojis)
            {
                Assert.Contains(EmojiDefinitions.CodeMap[emoji], unicodes);
            }
        }
    }
}
