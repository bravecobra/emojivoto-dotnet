using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Resources;
using System.Reflection;

namespace EmojiShared.Configuration
{
    public static class ResourceBuilderFactory
    {
        public static ResourceBuilder ConfigureResourceBuilder(ResourceBuilder resourceBuilder)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var entryAssemblyName = entryAssembly?.GetName();
            var versionAttribute = entryAssembly?.GetCustomAttributes(false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();
            var serviceName = entryAssemblyName?.Name ?? "unknown";
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
            resourceBuilder.AddService(serviceName, null, serviceVersion, serviceInstanceId: Environment.MachineName)
                .AddAttributes(attributes)
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector()
                .AddDetector(new DockerResourceDetector());
            return resourceBuilder;
        }
    }
}
