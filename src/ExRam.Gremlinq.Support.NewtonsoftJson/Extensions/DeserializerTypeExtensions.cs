using System.Collections.Concurrent;
using System.Reflection;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    internal static class DeserializerTypeExtensions
    {
        public readonly struct FluentForType
        {
            private readonly Type _type;
            private readonly IDeserializer _deserializer;

            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromClassDelegates = new();
            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromStructDelegates = new();

            public FluentForType(IDeserializer deserializer, Type type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = type.GetGenericArguments()[0];

                _type = type;
                _deserializer = deserializer;
            }

            public object? From<TSource>(TSource serialized, IGremlinQueryEnvironment environment)
            {
                return TryGetDelegate(typeof(TSource), _type) is Func<IDeserializer, TSource, IGremlinQueryEnvironment, object?> fromDelegate
                    ? fromDelegate(_deserializer, serialized, environment)
                    : default;
            }

            private static Delegate? TryGetDelegate(Type serializedType, Type requestedType)
            {
                var delegatesDict = requestedType.IsValueType
                    ? FromStructDelegates
                    : FromClassDelegates;

                return delegatesDict
                    .GetOrAdd(
                        (serializedType, requestedType),
                        static tuple =>
                        {
                            var (serializedType, requestedType) = tuple;

                            var methodName = requestedType.IsValueType
                                ? nameof(FromStruct)
                                : nameof(FromClass);

                            return typeof(FluentForType)
                                .GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)!
                                .MakeGenericMethod(serializedType, requestedType)
                                .Invoke(null, Array.Empty<object>()) as Delegate;
                        });
            }

            private static Func<IDeserializer, TSource, IGremlinQueryEnvironment, object?> FromClass<TSource, TRequested>()
                where TRequested : class => (deserializer, serialized, environment) => deserializer.TryDeserialize<TSource, TRequested>(serialized, environment, out var value)
                    ? value
                    : default;

            private static Func<IDeserializer, TSource, IGremlinQueryEnvironment, object?> FromStruct<TSource, TRequested>()
                where TRequested : struct => (deserializer, serialized, environment) => deserializer.TryDeserialize<TSource, TRequested>(serialized, environment, out var value)
                    ? value
                    : default(TRequested?);
        }

        public static FluentForType TryDeserialize(this IDeserializer deserializer, Type type) => new(deserializer, type);
    }
}
