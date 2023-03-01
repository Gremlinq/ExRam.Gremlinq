using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Transformation;

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

            public TRequested? From<TSource>(TSource serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSource, TRequested>(serialized, environment, out var value)
                ? value
                : default(TRequested?);
        }

        private sealed class FixedTypeConverterFactory<TStaticSerialized, TStaticRequested> : IConverterFactory
            where TStaticRequested : struct
        {
            private sealed class FixedTypeConverter<TSource> : IConverter<TSource, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> _func;

                public FixedTypeConverter(Func<TStaticSerialized, IGremlinQueryEnvironment, IDeserializer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSource serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
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
                return typeof(TRequested) == typeof(TStaticRequested) && typeof(TStaticSerialized).IsAssignableFrom(typeof(TSource))
                    ? (IConverter<TSource, TRequested>)(object)new FixedTypeConverter<TSource>(_func)
                    : null;
            }
        }

        public static FluentForStruct<TRequested> TryDeserialize<TRequested>(this IDeserializer deserializer)
            where TRequested : struct
        {
            return new FluentForStruct<TRequested>(deserializer);
        }

        public static IDeserializer Override<TSource, TRequested>(this IDeserializer deserializer, Func<TSource, IGremlinQueryEnvironment, IDeserializer, TRequested?> func)
            where TRequested : struct
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSource, TRequested>(func));
        }
    }
}
