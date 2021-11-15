using System;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetRequiredConfiguration(this IConfiguration configuration, string key)
        {
            return configuration[key] is { } value
                ? value
                : throw new InvalidOperationException($"Missing required configuration for {(configuration is ConfigurationSection section ? $"{section.Path}:{key}" : key)}.");
        }
    }
}
