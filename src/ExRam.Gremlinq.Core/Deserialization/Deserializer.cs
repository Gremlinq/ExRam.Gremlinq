using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class Deserializer
    {
        private sealed class IdentityConverterFactory : IConverterFactory
        {
            private sealed class IdentityConverter<TSerialized, TRequested> : IConverter<TSerialized, TRequested>
            {
                public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized is TRequested requested)
                    {
                        value = requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSerialized, TRequested> TryCreate<TSerialized, TRequested>() => new IdentityConverter<TSerialized, TRequested>();
        }

        private sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
        {
            private sealed class SingleItemArrayFallbackConverter<TSerialized, TRequestedArray, TRequestedArrayItem> : IConverter<TSerialized, TRequestedArray>
            {
                public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (recurse.TryDeserialize<TSerialized, TRequestedArrayItem>(serialized, environment, out var typedValue))
                    {
                        value = (TRequestedArray)(object) new[] { typedValue };
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IConverter<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class ToStringFallbackConverterFactory : IConverterFactory
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

        public static readonly IDeserializer Identity = new Transformer(ImmutableStack<IConverterFactory>.Empty)
            .Add(new IdentityConverterFactory());

        public static readonly IDeserializer Default = Identity
            .Add(new SingleItemArrayFallbackConverterFactory())
            .AddToStringFallback();

        public static IDeserializer AddToStringFallback(this IDeserializer deserializer) => deserializer
            .Add(new ToStringFallbackConverterFactory());
    }
}
