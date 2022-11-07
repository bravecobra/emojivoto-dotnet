using AutoMapper;

namespace EmojiSvc.Domain
{
    public class EmojiProfile : Profile
    {
        public EmojiProfile()
        {
            CreateMap<Emoji, Emojivoto.V1.Emoji>();
        }
    }
}
