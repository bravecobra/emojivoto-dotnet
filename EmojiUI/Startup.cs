using System;
using System.Reflection;
using EmojiUI.Services;
using EmojiUI.Services.Impl;
using EmojiUI.Shared;
using Emojivoto.V1;
using Fluxor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace EmojiUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor(options => options.DetailedErrors = true);
            services.AddTransient<IEmojiVoteService, EmojiVoteService>();
            services.AddGrpcClient<EmojiService.EmojiServiceClient>(o =>
                {
                    var emojisvcurl = _configuration["EMOJISVC_HOST"];
                    o.Address = new Uri(emojisvcurl);
                });

            services.AddGrpcClient<VotingService.VotingServiceClient>(o =>
                {
                    var emojisvcurl = _configuration["VOTINGSVC_HOST"];
                    o.Address = new Uri(emojisvcurl);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddFluxor(o => o
                .ScanAssemblies(typeof(Startup).Assembly)
                .UseRouting()
                .UseReduxDevTools()
                .AddMiddleware<LoggingMiddleware>());
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
                            options.RecordException = false;
                            options.EnableGrpcAspNetCoreSupport = true;
                        })
                        .AddXRayTraceId()
                        .AddAWSInstrumentation()
                        .AddHttpClientInstrumentation(options => options.RecordException = false)
                        .AddGrpcCoreInstrumentation()
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
                    var otelUri =_configuration["AWS_OTEL_URI"];
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
