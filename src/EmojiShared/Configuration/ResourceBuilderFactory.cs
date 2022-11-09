using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Resources;

namespace EmojiShared.Configuration
{
    public static class ResourceBuilderFactory
    {
        public static ResourceBuilder CreateResourceBuilder(IHostBuilder builder)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyName = entryAssembly?.GetName();
            var versionAttribute = entryAssembly?.GetCustomAttributes(false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();
            var serviceName = entryAssemblyName?.Name;
            var serviceVersion = versionAttribute?.InformationalVersion ?? entryAssemblyName?.Version?.ToString() ?? "unknown";
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "none";
            var attributes = new Dictionary<string, object>
            {
                ["host.name"] = Environment.MachineName,
                ["os.description"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                ["deployment.environment"] = environment,
                ["env"] = environment,
                ["version"] = serviceVersion,
                ["service"] = serviceName,
            };
            ActivitySourceFactory.GetActivitySource();
            return ResourceBuilder.CreateDefault()
                .AddService(serviceName, null ,serviceVersion, serviceInstanceId: Environment.MachineName)
                .AddAttributes(attributes)
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddDetector(new DockerResourceDetector());
        }

        public static ResourceBuilder CreateResourceBuilder(WebApplicationBuilder builder)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyName = entryAssembly?.GetName();
            var versionAttribute = entryAssembly?.GetCustomAttributes(false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();
            var serviceName = entryAssemblyName?.Name;
            var serviceVersion = versionAttribute?.InformationalVersion ?? entryAssemblyName?.Version?.ToString() ?? "unknown";
            var environment = builder.Environment.EnvironmentName.ToLowerInvariant();
            var attributes = new Dictionary<string, object>
            {
                ["host.name"] = Environment.MachineName,
                ["os.description"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                ["deployment.environment"] = environment,
                ["env"] = environment,
                ["version"] = serviceVersion,
                ["service"] = serviceName,
                ["application_type"] = "web" //autodetection datadog
            };

            return ResourceBuilder.CreateDefault()
                .AddService(serviceName, null, serviceVersion, serviceInstanceId: Environment.MachineName)
                .AddAttributes(attributes)
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddDetector(new DockerResourceDetector());
        }
    }
}
