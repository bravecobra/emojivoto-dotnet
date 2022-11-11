using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Resources;

namespace EmojiShared.Configuration
{
    public static class ResourceBuilderFactory
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static ResourceBuilder CreateResourceBuilder(IHostBuilder builder)
#pragma warning restore IDE0060 // Remove unused parameter
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
                ["service"] = serviceName ?? "unknown",
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
                ["service"] = serviceName ?? "unknown",
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
