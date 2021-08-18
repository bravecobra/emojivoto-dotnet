using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmojiSvc.Domain;
using Emojivoto.V1;
using Grpc.Core;
using Microsoft.Extensions.Logging;


namespace EmojiSvc.Services
{
    public class EmojiSvc: EmojiService.EmojiServiceBase
    {
        private readonly ILogger<EmojiSvc> _logger;
        private readonly IAllEmoji _allEmoji;
        private readonly IMapper _mapper;

        public EmojiSvc(ILogger<EmojiSvc> logger, IAllEmoji allEmoji, IMapper mapper)
        {
            _logger = logger;
            _allEmoji = allEmoji;
            _mapper = mapper;
        }

        public override Task<FindByShortcodeResponse> FindByShortcode(FindByShortcodeRequest request, ServerCallContext context)
        {
            var emoji = _allEmoji.WithShortcode(request.Shortcode) ?? null;

            Emojivoto.V1.Emoji? result = null;
            if (emoji != null)
            {
                _mapper.Map<Emojivoto.V1.Emoji>(emoji);
            }
            
            return Task.FromResult(new FindByShortcodeResponse
            {
                Emoji = result
            });
        }

        public override Task<ListAllEmojiResponse> ListAll(ListAllEmojiRequest request, ServerCallContext context)
        {
            var result = _mapper.Map<List<Emojivoto.V1.Emoji>>(_allEmoji.List());
            var response = new ListAllEmojiResponse();
            response.List.AddRange(result.AsEnumerable());
            return Task.FromResult(response);
        }
    }
}
