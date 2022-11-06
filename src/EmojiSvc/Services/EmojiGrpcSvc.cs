using AutoMapper;
using EmojiSvc.Domain;
using Emojivoto.V1;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmojiSvc.Services
{
    public class EmojiGrpcSvc : EmojiService.EmojiServiceBase
    {
        private readonly ILogger<EmojiGrpcSvc> _logger;
        private readonly IAllEmoji _allEmoji;
        private readonly IMapper _mapper;

        public EmojiGrpcSvc(ILogger<EmojiGrpcSvc> logger, IAllEmoji allEmoji, IMapper mapper)
        {
            _logger = logger;
            _allEmoji = allEmoji;
            _mapper = mapper;
        }

        public override Task<FindByShortcodeResponse> FindByShortcode(FindByShortcodeRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Finding {request.Shortcode}");
            var emoji = _allEmoji.WithShortcode(request.Shortcode) ?? null;

            Emojivoto.V1.Emoji? result = null;
            if (emoji != null)
            {
                result = _mapper.Map<Emojivoto.V1.Emoji>(emoji);
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
