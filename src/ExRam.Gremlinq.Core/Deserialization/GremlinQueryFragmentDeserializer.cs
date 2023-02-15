using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private readonly struct Option<T>
            {
                private Option(T value)
                {
                    HasValue = true;
                    Value = value;
                }

                public T Value { get; }
                public bool HasValue { get; }

                public static Option<T> None = new();
                public static Option<T> From(T value) => new(value);
            }

            private readonly ImmutableStack<IDeserializationTransformationFactory> _transformationFactories;
            private readonly ConcurrentDictionary<(Type, Type, Type), Delegate> _transformationDelegates = new();

            public GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformationFactory> transformationFactories)
            {
                _transformationFactories = transformationFactories;
            }

            public bool TryDeserialize<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value)
            {
                var maybeDeserializerDelegate = _transformationDelegates
                    .GetOrAdd(
                        (typeof(TSerialized), serialized!.GetType(), typeof(TRequested)),
                        static typeTuple =>
                        {
                            var (staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                            return (Delegate)typeof(GremlinQueryFragmentDeserializerImpl)
                                .GetMethod(nameof(GetDeserializationFunction), BindingFlags.Static | BindingFlags.NonPublic)!
                                .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)!
                                .Invoke(null, Array.Empty<object>())!;
                        }) as Func<GremlinQueryFragmentDeserializerImpl, TSerialized, IGremlinQueryEnvironment, Option<TRequested>>;

                if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(this, serialized, environment) is { HasValue: true, Value: { } optionValue })
                {
                    value = optionValue;
                    return true;
                }

                value = default;
                return false;
            }

            public IGremlinQueryFragmentDeserializer Override(IDeserializationTransformationFactory deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(_transformationFactories.Push(deserializer));
            }

            private static Func<GremlinQueryFragmentDeserializerImpl, TStaticSerialized, IGremlinQueryEnvironment, Option<TRequested>> GetDeserializationFunction<TStaticSerialized, TActualSerialized, TRequested>()
                where TActualSerialized : TStaticSerialized
            {
                return static (deserializer, staticSerialized, environment) =>
                {
                    if (staticSerialized is TActualSerialized actualSerialized)
                    {
                        foreach (var transformationFactory in deserializer._transformationFactories)
                        {
                            if (transformationFactory.TryCreate<TActualSerialized, TRequested>() is { } transformation)
                            {
                                if (transformation.Transform(actualSerialized, environment, deserializer, out var value))
                                    return Option<TRequested>.From(value);
                            }
                        }
                    }

                    return Option<TRequested>.None;
                };
            }
        }

        private sealed class SingleItemArrayFallbackDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class SingleItemArrayFallbackDeserializationTransformation<TSerialized, TRequestedArray, TRequestedArrayItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
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

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformationFactory>.Empty)
            .Override(DeserializationTransformationFactory.Identity);

        public static readonly IGremlinQueryFragmentDeserializer Default = Identity
            .Override(new SingleItemArrayFallbackDeserializationTransformationFactory())
            .AddToStringFallback();

        public static IGremlinQueryFragmentDeserializer AddToStringFallback(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override<object, string>(static (data, env, recurse) => data.ToString());
    }
}
