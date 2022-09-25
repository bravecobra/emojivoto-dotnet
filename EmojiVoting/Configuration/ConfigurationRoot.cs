using EmojiVoting.Application.Impl;
using EmojiVoting.Application;
using EmojiVoting.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmojiVoting.Persistence;
using Microsoft.EntityFrameworkCore;
using EmojiVoting.Persistence.Impl;

namespace EmojiVoting.Configuration
{
    public static class ConfigurationRoot
    {
        public static IServiceCollection AddConfigurationRoot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddTransient<IPollService, PollService>();
            services.AddSingleton(configuration);
            services.AddAutoMapper(typeof(VotingProfile));
            services.AddDbContext<VotingContext>(builder =>
            {
                builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                builder.EnableSensitiveDataLogging();
            });
            services.AddTransient<IVotingRepository, VotingRepository>();

            return services;
        }
    }
}
