using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class DeserializerStructExtensions
    {
        public readonly struct FluentForStruct<TTarget>
            where TTarget : struct
        {
            private readonly IDeserializer _deserializer;

            public FluentForStruct(IDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TTarget? From<TSource>(TSource source, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSource, TTarget>(source, environment, out var value)
                ? value
                : default(TTarget?);
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

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
                {
                    if (source is TStaticSerialized staticSerialized && _func(staticSerialized, environment, recurse) is { } requested)
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

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TTarget) == typeof(TStaticRequested) && typeof(TStaticSerialized).IsAssignableFrom(typeof(TSource))
                    ? (IConverter<TSource, TTarget>)(object)new FixedTypeConverter<TSource>(_func)
                    : null;
            }
        }

        public static FluentForStruct<TTarget> TryDeserialize<TTarget>(this IDeserializer deserializer)
            where TTarget : struct
        {
            return new FluentForStruct<TTarget>(deserializer);
        }

        public static IDeserializer Override<TSource, TTarget>(this IDeserializer deserializer, Func<TSource, IGremlinQueryEnvironment, IDeserializer, TTarget?> func)
            where TTarget : struct
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSource, TTarget>(func));
        }
    }
}
