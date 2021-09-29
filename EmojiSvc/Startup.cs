﻿using System;
using System.Reflection;
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
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace EmojiSvc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<IEmojiRepo, InMemoryAllEmoji>();
            services.AddTransient<IAllEmoji, AllEmoji>();
            services.AddAutoMapper(typeof(EmojiProfile));
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true); //AWS
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(Assembly.GetEntryAssembly()?.GetName().Name)
                .AddTelemetrySdk();
            services.AddOpenTelemetryTracing(
                (builder) =>
                {
                    builder
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                            options.EnableGrpcAspNetCoreSupport = true;
                        })
                        .AddXRayTraceId()
                        .AddAWSInstrumentation()
                        .AddGrpcCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .SetResourceBuilder(resourceBuilder);
                    var consoleExport = _configuration.GetValue<bool>("CONSOLE_EXPORT");
                    if (consoleExport)
                    {
                        builder.AddConsoleExporter();
                    }
                    var jaegerHost = _configuration.GetValue<string>("JAEGER_HOST");
                    var jaegerPort = _configuration.GetValue<int>("JAEGER_PORT");
                    if (!string.IsNullOrEmpty(jaegerHost))
                    {
                        builder.AddJaegerExporter(options =>
                        {
                            options.AgentHost = jaegerHost;
                            options.AgentPort = jaegerPort;
                        });
                    }
                    var otelUri = _configuration["AWS_OTEL_URI"];
                    if (!string.IsNullOrEmpty(otelUri))
                    {
                        builder.AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri(otelUri);
                        });
                    }
                });
            Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
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
