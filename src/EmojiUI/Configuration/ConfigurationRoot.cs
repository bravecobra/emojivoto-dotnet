using EmojiUI.Services.Impl;
using EmojiUI.Shared;
using Fluxor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using EmojiUI.Services;
using Emojivoto.V1;

namespace EmojiUI.Configuration
{
    public static class ConfigurationRoot
    {
        public static IServiceCollection AddConfigurationRoot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddSignalR();
            services.AddServerSideBlazor(options => options.DetailedErrors = true);
            services.AddTransient<IEmojiVoteService, EmojiVoteService>();
            services.AddGrpcClient<EmojiService.EmojiServiceClient>(o =>
            {
                var emojisvcurl = configuration["EMOJISVC_HOST"];
                o.Address = new Uri(emojisvcurl);
            });

            services.AddGrpcClient<VotingService.VotingServiceClient>(o =>
            {
                var emojisvcurl = configuration["VOTINGSVC_HOST"];
                o.Address = new Uri(emojisvcurl);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddFluxor(o => o
                .ScanAssemblies(typeof(Program).Assembly)
                .WithLifetime(StoreLifetime.Scoped)
                .UseRouting()
                .UseReduxDevTools()
                .AddMiddleware<LoggingMiddleware>());
            return services;
        }
    }
}
