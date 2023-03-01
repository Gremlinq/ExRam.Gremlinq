using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Transformation
{
    public static class Transformer
    {
        private sealed class TransformerImpl : ITransformer
        {
            private readonly struct Option<T>
            {
                public static readonly Option<T> None = new();

                private Option(T value)
                {
                    Value = value;
                    HasValue = true;
                }

                public T Value { get; }
                public bool HasValue { get; }

                public static Option<T> From(T value) => new(value);
            }

            private readonly ImmutableStack<IConverterFactory> _converterFactories;
            private readonly ConcurrentDictionary<(Type, Type, Type), Delegate> _conversionDelegates = new();

            public TransformerImpl(ImmutableStack<IConverterFactory> converterFactories)
            {
                _converterFactories = converterFactories;
            }

            public ITransformer Add(IConverterFactory converterFactory)
            {
                return new TransformerImpl(_converterFactories.Push(converterFactory));
            }

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
            {
                if (source is { } actualSerialized)
                {
                    var maybeDeserializerDelegate = _conversionDelegates
                        .GetOrAdd(
                            (typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                            static (typeTuple, @this) =>
                            {
                                var (staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                                return (Delegate)typeof(TransformerImpl)
                                    .GetMethod(nameof(GetTransformationFunction), BindingFlags.Instance | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)
                                    .Invoke(@this, Array.Empty<object>())!;
                            },
                            this) as Func<TSource, IGremlinQueryEnvironment, Option<TTarget>>;

                    if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(source, environment) is { HasValue: true, Value: { } optionValue })
                    {
                        value = optionValue;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            private Func<TStaticSource, IGremlinQueryEnvironment, Option<TTarget>> GetTransformationFunction<TStaticSource, TActualSource, TTarget>()
                where TActualSource : TStaticSource
            {
                var converters = _converterFactories
                    .Select(static factory => factory.TryCreate<TActualSource, TTarget>())
                    .Where(static converter => converter != null)
                    .Select(static converter => converter!)
                    .ToArray();

                return (staticSerialized, environment) =>
                {
                    if (staticSerialized is TActualSource actualSerialized)
                        foreach (var converter in converters)
                            if (converter.TryConvert(actualSerialized, environment, this, out var value))
                                return Option<TTarget>.From(value);

                    return Option<TTarget>.None;
                };
            }
        }

        public static readonly ITransformer Identity = new TransformerImpl(ImmutableStack<IConverterFactory>.Empty)
            .Add(new IdentityConverterFactory());
    }
}
