using Microsoft.AspNetCore.Builder;
using System;
using EmojiShared.Configuration;
using EmojiVoting.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmojiVoting.Persistence;
using EmojiVoting.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmojiVoting
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //To allow grpc HTTP2 traffic over non-SSL
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddConfigurationRoot(builder.Configuration);
            // Add Logging
            builder.AddCustomLogging();
            // Add Telemetry
            builder.Services.AddCustomTelemetry(builder.Configuration, Array.Empty<string>());

            builder.Services.AddHealthChecks();

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseMiddleware<LogContextMiddleware>();
            app.UseCustomErrorhandling();
            app.UseRouting();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<VotingDbContext>();
                db.Database.Migrate();
            }
            app.AddMetricsEndpoint();
            app.MapGrpcService<VotingGrpcSvc>();
            app.MapHealthChecks("/health/startup");
            app.MapHealthChecks("/healthz");
            app.MapHealthChecks("/ready");
            app.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
            app.Run();
        }
    }
}
