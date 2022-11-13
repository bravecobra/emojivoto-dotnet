using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EmojiShared.Configuration
{
    public static class WebApplicationExtensions
    {
        public static WebApplication AddMetricsEndpoint(this WebApplication app)
        {
            var metricsExporter = app.Configuration.GetValue<string>("UseMetricsExporter")!.ToLowerInvariant();

            if (metricsExporter == "prometheus")
            {
                app.UseOpenTelemetryPrometheusScrapingEndpoint();
            }
            return app;
        }

        public static WebApplication UseCustomErrorhandling(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            context.Response.StatusCode,
                            contextFeature.Error.Message,
                            TraceID = Activity.Current?.TraceId.ToString(),
                        }));
                    }
                });
            });
            return app;
        }
    }
}
