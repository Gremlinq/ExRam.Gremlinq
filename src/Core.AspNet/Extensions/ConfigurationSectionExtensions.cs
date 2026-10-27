using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal static class ConfigurationSectionExtensions
    {
        public static Version? GetVersion(this IConfigurationSection section) => section.Value is { Length: > 0 } value
            ? Version.TryParse(value, out var version)
                ? version
                : throw new InvalidOperationException($"The value '{value}' of {section.Path} is not a version.")
            : null;
    }
}
