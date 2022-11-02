using EmojiSvc.Domain.Impl;
using EmojiSvc.Domain;
using EmojiSvc.Persistence.Impl;
using EmojiSvc.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmojiSvc.Configuration
{
    public static class ConfigurationRoot
    {
        public static IServiceCollection AddConfigurationRoot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddTransient<IEmojiRepo, DatabaseEmojiRepo>();
            services.AddTransient<IAllEmoji, AllEmoji>();
            services.AddAutoMapper(typeof(EmojiProfile));
            services.AddDbContext<EmojiDbContext>(builder =>
            {
                builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                builder.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
