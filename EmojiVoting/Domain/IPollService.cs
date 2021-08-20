using System.Collections.Generic;

namespace EmojiVoting.Domain
{
    public interface IPollService
    {
        void Vote(string choice);
        List<Result> Results();
    }
}