using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using EmojiUI.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmojiUI
{
    static class Program
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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            app.AddMetricsEndpoint();
            app.Run();
        }
    }
}
