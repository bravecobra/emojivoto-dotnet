using System.Collections.Generic;
using System.Threading.Tasks;
using EmojiVoting.Domain;

namespace EmojiVoting.Application
{
    public interface IPollService
    {
        Task Vote(string choice);
        Task<List<Result>> Results();
    }
}