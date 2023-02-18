using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class Deserializer
    {
        private sealed class DeserializerImpl : IDeserializer
        {
            private readonly struct Option<T>
            {
                private Option(T value)
                {
                    Value = value;
                    HasValue = true;
                }

                public T Value { get; }
                public bool HasValue { get; }

                public static Option<T> None = new();
                public static Option<T> From(T value) => new(value);
            }

            private readonly ImmutableStack<IConverterFactory> _transformationFactories;
            private readonly ConcurrentDictionary<(Type, Type, Type), Delegate> _transformationDelegates = new();

            public DeserializerImpl(ImmutableStack<IConverterFactory> transformationFactories)
            {
                _transformationFactories = transformationFactories;
            }

            public bool TryDeserialize<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value)
            {
                if (serialized is { } actualSerialized)
                {
                    var maybeDeserializerDelegate = _transformationDelegates
                        .GetOrAdd(
                            (typeof(TSerialized), actualSerialized.GetType(), typeof(TRequested)),
                            static (typeTuple, @this) =>
                            {
                                var (staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                                return (Delegate)typeof(DeserializerImpl)
                                    .GetMethod(nameof(GetDeserializationFunction), BindingFlags.Instance | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)!
                                    .Invoke(@this, Array.Empty<object>())!;
                            },
                            this) as Func<TSerialized, IGremlinQueryEnvironment, Option<TRequested>>;

                    if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(serialized, environment) is { HasValue: true, Value: { } optionValue })
                    {
                        value = optionValue;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            public IDeserializer Add(IConverterFactory deserializer)
            {
                return new DeserializerImpl(_transformationFactories.Push(deserializer));
            }

            private Func<TStaticSerialized, IGremlinQueryEnvironment, Option<TRequested>> GetDeserializationFunction<TStaticSerialized, TActualSerialized, TRequested>()
                where TActualSerialized : TStaticSerialized
            {
                var transformations = _transformationFactories
                    .Select(factory => factory.TryCreate<TActualSerialized, TRequested>())
                    .Where(transformation => transformation != null)
                    .Select(transformation => transformation!)
                    .ToArray();

                return (staticSerialized, environment) =>
                {
                    if (staticSerialized is TActualSerialized actualSerialized)
                    {
                        foreach (var transformation in transformations)
                        {
                            if (transformation.Transform(actualSerialized, environment, this, out var value))
                                return Option<TRequested>.From(value);
                        }
                    }

                    return Option<TRequested>.None;
                };
            }
        }

        private sealed class IdentityConverterFactory : IConverterFactory
        {
            private sealed class IdentityConverter<TSerialized, TRequested> : IConverter<TSerialized, TRequested>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized is TRequested requested)
                    {
                        value = requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>() => new IdentityConverter<TSerialized, TRequested>();
        }

        private sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
        {
            private sealed class SingleItemArrayFallbackConverter<TSerialized, TRequestedArray, TRequestedArrayItem> : IConverter<TSerialized, TRequestedArray>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
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

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IConverter<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class ToStringFallbackConverterFactory : IConverterFactory
        {
            private sealed class ToStringFallbackConverter<TSerialized> : IConverter<TSerialized, string>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out string? value)
                {
                    if (serialized?.ToString() is { } requested)
                    {
                        value = requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested) == typeof(string)
                    ? (IConverter<TSerialized, TRequested>)(object)new ToStringFallbackConverter<TSerialized>()
                    : default;
            }
        }

        public static readonly IDeserializer Identity = new DeserializerImpl(ImmutableStack<IConverterFactory>.Empty)
            .Add(new IdentityConverterFactory());

        public static readonly IDeserializer Default = Identity
            .Add(new SingleItemArrayFallbackConverterFactory())
            .AddToStringFallback();

        public static IDeserializer AddToStringFallback(this IDeserializer deserializer) => deserializer
            .Add(new ToStringFallbackConverterFactory());
    }
}
