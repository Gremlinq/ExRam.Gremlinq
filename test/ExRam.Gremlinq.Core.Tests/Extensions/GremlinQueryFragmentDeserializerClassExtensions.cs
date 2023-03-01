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

            public TRequested? From<TSource>(TSource serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSource, TRequested>(serialized, environment, out var value)
                ? value
                : default;
        }

        private sealed class FixedTypeConverterFactory<TStaticSerialized, TStaticRequested> : IConverterFactory
           where TStaticRequested : class
        {
            private sealed class FixedTypeConverter<TSource> : IConverter<TSource, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> _func;

                public FixedTypeConverter(Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSource serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested? value)
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

            public IConverter<TSource, TRequested>? TryCreate<TSource, TRequested>()
            {
                return ((typeof(TSource).IsAssignableFrom(typeof(TStaticSerialized)) || typeof(TStaticSerialized).IsAssignableFrom(typeof(TSource))) && (typeof(TRequested) == typeof(TStaticRequested)))
                    ? (IConverter<TSource, TRequested>)(object)new FixedTypeConverter<TSource>(_func)
                    : null;
            }
        }

        public static FluentForClass<TRequested> TryDeserialize<TRequested>(this IDeserializer deserializer)
            where TRequested : class
        {
            return new FluentForClass<TRequested>(deserializer);
        }

        public static IDeserializer Override<TSource, TRequested>(this IDeserializer deserializer, Func<TSource, IGremlinQueryEnvironment, IDeserializer, TRequested?> func)
            where TRequested : class
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSource, TRequested>(func));
        }
    }
}
