using EmojiVoting.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmojiVoting.Application
{
    public interface IPollService
    {
        Task Vote(string choice);
        Task<List<Result>> Results();
    }
}