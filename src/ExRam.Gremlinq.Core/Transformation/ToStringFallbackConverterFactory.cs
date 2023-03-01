using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class ToStringFallbackConverterFactory : IConverterFactory
    {
        private sealed class ToStringFallbackConverter<TSerialized> : IConverter<TSerialized, string>
        {
            public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out string? value)
            {
                if (serialized?.ToString() is { } requested)
                {
                    value = requested;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
        {
            return typeof(TRequested) == typeof(string)
                ? (IConverter<TSerialized, TRequested>)(object)new ToStringFallbackConverter<TSerialized>()
                : default;
        }
    }
}
