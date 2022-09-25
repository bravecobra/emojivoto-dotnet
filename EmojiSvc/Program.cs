using EmojiSvc.Configuration;
using EmojiSvc.Persistence;
using EmojiSvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using EmojiShared.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EmojiSvc
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true); //AWS
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddConfigurationRoot(builder.Configuration);
            var resourceBuilder = ResourceBuilderFactory.CreateResourceBuilder(builder);
            // Add Logging
            builder.AddCustomLogging(resourceBuilder);
            // Add Metrics
            builder.Services.AddCustomMetrics(builder.Configuration, resourceBuilder);
            // Add Traces
            builder.Services.AddCustomTracing(builder.Configuration, resourceBuilder, Array.Empty<string>());

            builder.Services.AddHealthChecks();

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseMiddleware<LogContextMiddleware>();
            app.UseCustomErrorhandling();

            app.UseRouting();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EmojiDbContext>();
                db.Database.Migrate();
            }
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EmojiGrpcSvc>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
                endpoints.MapHealthChecks("/health/startup");
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapHealthChecks("/ready");
                endpoints.MapControllers();
            });

            app.AddMetricsEndpoint();
            app.Run();
        }
    }
}
