using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class DeserializerClassExtensions
    {
        public readonly struct FluentForClass<TRequested>
            where TRequested : class
        {
            private readonly IDeserializer _deserializer;

            public FluentForClass(IDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TRequested? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TRequested>(serialized, environment, out var value)
                ? value
                : default;
        }

        private sealed class FixedTypeConverterFactory<TStaticSerialized, TStaticRequested> : IConverterFactory
           where TStaticRequested : class
        {
            private sealed class FixedTypeConverter<TSerialized> : IConverter<TSerialized, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> _func;

                public FixedTypeConverter(Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested? value)
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
                return ((typeof(TSerialized).IsAssignableFrom(typeof(TStaticSerialized)) || typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized))) && (typeof(TRequested) == typeof(TStaticRequested)))
                    ? (IConverter<TSerialized, TRequested>)(object)new FixedTypeConverter<TSerialized>(_func)
                    : null;
            }
        }

        public static FluentForClass<TRequested> TryDeserialize<TRequested>(this IDeserializer deserializer)
            where TRequested : class
        {
            return new FluentForClass<TRequested>(deserializer);
        }

        public static IDeserializer Override<TSerialized, TRequested>(this IDeserializer deserializer, Func<TSerialized, IGremlinQueryEnvironment, IDeserializer, TRequested?> func)
            where TRequested : class
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSerialized, TRequested>(func));
        }
    }
}
