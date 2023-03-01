using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class Transformer : IDeserializer
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

        public Transformer(ImmutableStack<IConverterFactory> converterFactories)
        {
            _converterFactories = converterFactories;
        }

        public bool TryDeserialize<TSource, TTarget>(TSource serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
        {
            if (serialized is { } actualSerialized)
            {
                var maybeDeserializerDelegate = _conversionDelegates
                    .GetOrAdd(
                        (typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                        static (typeTuple, @this) =>
                        {
                            var (staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                            return (Delegate)typeof(Transformer)
                                .GetMethod(nameof(GetTransformationFunction), BindingFlags.Instance | BindingFlags.NonPublic)!
                                .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)
                                .Invoke(@this, Array.Empty<object>())!;
                        },
                        this) as Func<TSource, IGremlinQueryEnvironment, Option<TTarget>>;

                if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(serialized, environment) is { HasValue: true, Value: { } optionValue })
                {
                    value = optionValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public IDeserializer Add(IConverterFactory converterFactory)
        {
            return new Transformer(_converterFactories.Push(converterFactory));
        }

        private Func<TStaticSerialized, IGremlinQueryEnvironment, Option<TRequested>> GetTransformationFunction<TStaticSerialized, TActualSerialized, TRequested>()
            where TActualSerialized : TStaticSerialized
        {
            var converters = _converterFactories
                .Select(static factory => factory.TryCreate<TActualSerialized, TRequested>())
                .Where(static converter => converter != null)
                .Select(static converter => converter!)
                .ToArray();

            return (staticSerialized, environment) =>
            {
                if (staticSerialized is TActualSerialized actualSerialized)
                    foreach (var converter in converters)
                        if (converter.TryConvert(actualSerialized, environment, this, out var value))
                            return Option<TRequested>.From(value);

                return Option<TRequested>.None;
            };
        }
    }
}
