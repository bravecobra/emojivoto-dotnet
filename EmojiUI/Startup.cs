using System;
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
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace EmojiUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor(options => options.DetailedErrors = true);
            services.AddTransient<IEmojiVoteService, EmojiVoteService>();
            services
                .AddGrpcClient<EmojiService.EmojiServiceClient>(o =>
                {
                    var emojisvcurl = Configuration["EMOJISVC_HOST"];
                    o.Address = new Uri(emojisvcurl);
                });

            services
                .AddGrpcClient<VotingService.VotingServiceClient>(o =>
                {
                    var emojisvcurl = Configuration["VOTINGSVC_HOST"];
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
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService("EmojiUI")
                .AddTelemetrySdk();
            services.AddOpenTelemetryTracing(
                (builder) => builder
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnableGrpcAspNetCoreSupport = true;
                    })
                    .AddGrpcCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddConsoleExporter()
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
