using System;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetRequiredConfiguration(this IConfiguration configuration, string key)
        {
            if (configuration[key] is { } value)
                return value;

            throw new InvalidOperationException($"Missing required configuration for {key}.");
        }
    }
}
