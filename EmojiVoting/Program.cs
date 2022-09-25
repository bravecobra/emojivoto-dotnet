using Microsoft.AspNetCore.Builder;
using System;
using EmojiSvc.Configuration;
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
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddConfigurationRoot(builder.Configuration);
            var resourceBuilder = ResourceBuilderFactory.CreateResourceBuilder(builder);
            // Add Logging
            builder.AddCustomLogging(resourceBuilder);
            // Add Metrics
            builder.Services.AddCustomMetrics(builder.Configuration, resourceBuilder);
            // Add Traces
            builder.Services.AddCustomTracing(builder.Configuration, resourceBuilder);

            builder.Services.AddHealthChecks();

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseMiddleware<LogContextMiddleware>();
            app.UseCustomErrorhandling();
            app.UseRouting();
            app.UseAuthorization();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<VotingContext>();
                db.Database.Migrate();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<VotingGrpcSvc>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
            app.AddMetricsEndpoint();
            app.Run();
            // Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            // Activity.ForceDefaultIdFormat = true;
            // CreateHostBuilder(args).Build().Run();
        }
    }
}
