using EmojiShared.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmojiVoteBot.Services.Impl
{
    class EmojiVoteRestService : IEmojiVoteService
    {
        private readonly ILogger<EmojiVoteRestService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public EmojiVoteRestService(ILogger<EmojiVoteRestService> logger, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Emoji>> ListEmojis()
        {
            using var activity = ActivitySourceFactory.GetActivitySource().StartActivity();
            using var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(new Uri($"{_configuration["WEB_HOST"]}/api/Emojis"));
            response.EnsureSuccessStatusCode();
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                try
                {
                    return await JsonSerializer.DeserializeAsync<List<Emoji>>(contentStream, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Emoji>();}
                
                catch (JsonException) // Invalid JSON
                {
                    _logger.LogError("Invalid JSON.");
                }
            }
            else
            {
                _logger.LogError("HTTP Response was invalid and cannot be deserialised.");
            }
            return new List<Emoji>();
        }

        public async Task<Emoji?> FindByShortCode(string shortcode)
        {
            using var activity = ActivitySourceFactory.GetActivitySource().StartActivity();
            using var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(new Uri($"{_configuration["WEB_HOST"]}/api/Emojis/{shortcode}"));
            response.EnsureSuccessStatusCode();
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                try
                {
                    return await JsonSerializer.DeserializeAsync<Emoji>(contentStream, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException) // Invalid JSON
                {
                    _logger.LogError("Invalid JSON.");
                }
            }
            else
            {
                _logger.LogError("HTTP Response was invalid and cannot be deserialised.");
            }
            return null;
        }

        public async Task<bool> Vote(string choice)
        {
            using var activity = ActivitySourceFactory.GetActivitySource().StartActivity();
            using var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(new Uri($"{_configuration["WEB_HOST"]}/api/Vote?choice={choice}"), new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("StatusCode: {StatusCode}", response.StatusCode.ToString());
            return true;
        }
    }
}