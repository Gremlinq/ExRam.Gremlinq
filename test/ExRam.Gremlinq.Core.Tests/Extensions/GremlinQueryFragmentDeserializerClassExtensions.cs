using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core
{
    public static class DeserializerClassExtensions
    {
        public readonly struct FluentForClass<TTarget>
            where TTarget : class
        {
            private readonly ITransformer _deserializer;

            public FluentForClass(ITransformer deserializer)
            {
                _deserializer = deserializer;
            }

            public TTarget? From<TSource>(TSource source, IGremlinQueryEnvironment environment) => _deserializer.TryTransform<TSource, TTarget>(source, environment, out var value)
                ? value
                : default;
        }

        private sealed class FixedTypeConverterFactory<TStaticSerialized, TStaticRequested> : IConverterFactory
           where TStaticRequested : class
        {
            private sealed class FixedTypeConverter<TSource> : IConverter<TSource, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, ITransformer, TStaticRequested?> _func;

                public FixedTypeConverter(Func<TStaticSerialized, IGremlinQueryEnvironment, ITransformer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TStaticRequested? value)
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

            private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, ITransformer, TStaticRequested?> _func;

            public FixedTypeConverterFactory(Func<TStaticSerialized, IGremlinQueryEnvironment, ITransformer, TStaticRequested?> func)
            {
                _func = func;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return ((typeof(TSource).IsAssignableFrom(typeof(TStaticSerialized)) || typeof(TStaticSerialized).IsAssignableFrom(typeof(TSource))) && (typeof(TTarget) == typeof(TStaticRequested)))
                    ? (IConverter<TSource, TTarget>)(object)new FixedTypeConverter<TSource>(_func)
                    : null;
            }
        }

        public static FluentForClass<TTarget> TryDeserialize<TTarget>(this ITransformer deserializer)
            where TTarget : class
        {
            return new FluentForClass<TTarget>(deserializer);
        }

        public static ITransformer Override<TSource, TTarget>(this ITransformer deserializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, TTarget?> func)
            where TTarget : class
        {
            return deserializer
                .Add(new FixedTypeConverterFactory<TSource, TTarget>(func));
        }
    }
}
