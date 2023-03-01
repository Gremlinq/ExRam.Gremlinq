using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class DeserializerStructExtensions
    {
        public readonly struct FluentForStruct<TRequested>
            where TRequested : struct
        {
            private readonly IDeserializer _deserializer;

            public FluentForStruct(IDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TRequested? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TRequested>(serialized, environment, out var value)
                ? value
                : default(TRequested?);
        }

        private sealed class FixedTypeConverterFactory<TStaticSerialized, TStaticRequested> : IConverterFactory
            where TStaticRequested : struct
        {
            private sealed class FixedTypeConverter<TSerialized> : IConverter<TSerialized, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> _func;

                public FixedTypeConverter(Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
                {
                    if (serialized is TStaticSerialized staticSerialized && _func(staticSerialized, environment, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> _func;

            public FixedTypeConverterFactory(Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> func)
            {
                _func = func;
            }

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested) == typeof(TStaticRequested) && typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized))
                    ? (IConverter<TSerialized, TRequested>)(object)new FixedTypeConverter<TSerialized>(_func)
                    : null;
            }
        }

        public static FluentForStruct<TRequested> TryDeserialize<TRequested>(this IDeserializer deserializer)
            where TRequested : struct
        {
            return new FluentForStruct<TRequested>(deserializer);
        }

        public static IDeserializer Override<TSerialized, TRequested>(this IDeserializer deserializer, Func<TSerialized, IGremlinQueryEnvironment, IDeserializer, TRequested?> func)
            where TRequested : struct
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSerialized, TRequested>(func));
        }
    }
}
