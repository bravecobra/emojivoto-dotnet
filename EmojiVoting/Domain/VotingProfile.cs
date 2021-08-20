using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
