using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmojiSvc.Domain;
using EmojiSvc.Domain.Impl;
using EmojiSvc.Persistence;
using EmojiSvc.Persistence.Impl;
using EmojiSvc.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace EmojiSvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<IEmojiRepo, InMemoryAllEmoji>();
            services.AddTransient<IAllEmoji, AllEmoji>();
            services.AddAutoMapper(typeof(EmojiProfile));
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService("EmojiSvc");
            services.AddOpenTelemetryTracing(
                (builder) => builder
                    .AddAspNetCoreInstrumentation(options => options.EnableGrpcAspNetCoreSupport = true)
                    .AddGrpcCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "jaeger";
                        options.AgentPort = 6831;
                    })
                    .SetResourceBuilder(resourceBuilder)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EmojiGrpcSvc>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
