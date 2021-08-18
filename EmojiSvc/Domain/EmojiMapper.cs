using AutoMapper;

namespace EmojiSvc.Domain
{
    public class EmojiMapper: Profile
    {
        public EmojiMapper()
        {
            CreateMap<Emoji, Emojivoto.V1.Emoji>();
        }
    }
}
