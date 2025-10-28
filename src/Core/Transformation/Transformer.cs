using System.Collections.Concurrent;
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

            private readonly TransformerImpl _recurse;
            private readonly FastImmutableList<IConverterFactory> _converterFactories;
            private readonly ConcurrentDictionary<(IGremlinQueryEnvironment, Type, Type, Type), Delegate> _conversionDelegates = new();

            public TransformerImpl(FastImmutableList<IConverterFactory> converterFactories)
            {
                _recurse = this;
                _converterFactories = converterFactories;
            }

            private TransformerImpl(FastImmutableList<IConverterFactory> converterFactories, TransformerImpl recurse) : this(converterFactories)
            {
                _recurse = recurse;
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
                            (environment, typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                            static (typeTuple, @this) =>
                            {
                                var (environment, staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                                return (Delegate)typeof(TransformerImpl)
                                    .GetMethod(nameof(GetTransformationFunction), BindingFlags.Instance | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)
                                    .Invoke(@this, [environment])!;
                            },
                            this) as Func<TSource, Option<TTarget>>;

                    if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(source) is { HasValue: true, Value: { } optionValue })
                    {
                        value = optionValue;
                        return true;
                    }

                    if (source is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            private Func<TStaticSource, Option<TTarget>> GetTransformationFunction<TStaticSource, TActualSource, TTarget>(IGremlinQueryEnvironment environment)
                where TActualSource : TStaticSource
            {
                var stack = _converterFactories;
                var list = new List<(IConverter<TActualSource, TTarget> converter, TransformerImpl overridden)>();

                for (var i = 1; i <= stack.Count; i++)
                {
                    var converterFactory = stack[^i];

                    if (converterFactory.TryCreate<TActualSource, TTarget>(environment) is { } converter)
                        list.Add((converter, new TransformerImpl(stack[0..^i], this)));
                }

                var converters = list
                    .ToArray();

                return (staticSerialized) =>
                {
                    if (staticSerialized is TActualSource actualSerialized)
                    {
                        foreach (var converter in converters)
                        {
                            if (converter.converter.TryConvert(actualSerialized, converter.overridden, _recurse, out var value))
                                return Option<TTarget>.From(value);
                        }
                    }

                    return Option<TTarget>.None;
                };
            }
        }

        public static readonly ITransformer Empty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty);

        internal static readonly ITransformer SerializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(160));

        internal static readonly ITransformer DeserializerEmpty = new TransformerImpl(FastImmutableList<IConverterFactory>.Empty.EnsureCapacity(32));
    }
}
