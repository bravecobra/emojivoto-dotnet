using AutoMapper;
using Emojivoto.V1;

namespace EmojiVoting.Domain
{
    public class VotingProfile: Profile
    {
        public VotingProfile()
        {
            CreateMap<Result, VotingResult>();
        }
    }
}
