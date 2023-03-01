using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class ToStringFallbackConverterFactory : IConverterFactory
    {
        private sealed class ToStringFallbackConverter<TSource> : IConverter<TSource, string>
        {
            public bool TryConvert(TSource serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out string? value)
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

        public IConverter<TSource, TRequested>? TryCreate<TSource, TRequested>()
        {
            return typeof(TRequested) == typeof(string)
                ? (IConverter<TSource, TRequested>)(object)new ToStringFallbackConverter<TSource>()
                : default;
        }
    }
}
