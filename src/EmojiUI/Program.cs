using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using EmojiShared.Configuration;
using EmojiUI.Configuration;
using EmojiUI.Controllers;
using EmojiUI.Shared.Store.FetchEmojies;
using Microsoft.Extensions.DependencyInjection;

namespace EmojiUI
{
    static class Program
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
            builder.Services.AddCustomTelemetry(builder.Configuration,
                new []
                {
                    nameof(Effects),
                    nameof(EmojisController),
                    nameof(VoteController)
                });

            builder.Services.AddHealthChecks();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var app = builder.Build();
            app.UseMiddleware<LogContextMiddleware>();
            app.UseCustomErrorhandling();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseAuthorization();

            app.AddMetricsEndpoint();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHealthChecks("/health/startup");
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapHealthChecks("/ready");
            });
            app.Run();
        }
    }
}
