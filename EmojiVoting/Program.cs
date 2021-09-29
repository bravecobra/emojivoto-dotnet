using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace EmojiVoting
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureLogging(builder => builder.AddOpenTelemetry())
                .UseSerilog((context, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithAssemblyName()
                        .Enrich.WithAssemblyVersion()
                        .Enrich.WithAssemblyInformationalVersion()
                        .Enrich.WithEnvironment(context.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProcessId()
                        .Enrich.WithProcessName()
                        .Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .Enrich.WithSpan()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console(new RenderedCompactJsonFormatter())
                        .WriteTo.Seq(context.Configuration["SEQ_URI"]))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
