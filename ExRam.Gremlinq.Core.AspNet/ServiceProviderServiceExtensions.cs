using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceProviderServiceExtensions
    {
        public static T GetServiceOrThrow<T>(this IServiceProvider provider)
        {
            return provider.GetService<T>() ?? throw new InvalidOperationException($"Cannot resolve service of type {typeof(T)}. Make sure it is registered properly.");
        }
    }
}
