using System.Collections.Generic;

namespace EmojiSvc.Domain
{
    public interface IAllEmoji
    {
        Emoji? WithShortcode(string shortcode);
        List<Emoji> List();
    }
}