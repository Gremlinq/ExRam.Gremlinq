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

            public object? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment)
            {
                return TryGetDelegate(typeof(TSerialized), _type) is Func<IDeserializer, TSerialized, IGremlinQueryEnvironment, object?> fromDelegate
                    ? fromDelegate(_deserializer, serialized, environment)
                    : default;
            }

            private static Delegate? TryGetDelegate(Type serializedType, Type fragmentType)
            {
                var delegatesDict = fragmentType.IsValueType
                    ? FromStructDelegates
                    : FromClassDelegates;

                return delegatesDict
                    .GetOrAdd(
                        (serializedType, fragmentType),
                        static tuple =>
                        {
                            var (serializedType, fragmentType) = tuple;

                            var methodName = fragmentType.IsValueType
                                ? nameof(FromStruct)
                                : nameof(FromClass);

                            return typeof(FluentForType)
                                .GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)!
                                .MakeGenericMethod(serializedType, fragmentType)
                                .Invoke(null, Array.Empty<object>()) as Delegate;
                        });
            }

            private static Func<IDeserializer, TSerialized, IGremlinQueryEnvironment, object?> FromClass<TSerialized, TFragment>()
                where TFragment : class => (deserializer, serialized, environment) => deserializer.TryDeserialize<TSerialized, TFragment>(serialized, environment, out var value)
                    ? value
                    : default;

            private static Func<IDeserializer, TSerialized, IGremlinQueryEnvironment, object?> FromStruct<TSerialized, TFragment>()
                where TFragment : struct => (deserializer, serialized, environment) => deserializer.TryDeserialize<TSerialized, TFragment>(serialized, environment, out var value)
                    ? value
                    : default(TFragment?);
        }

        public static FluentForType TryDeserialize(this IDeserializer deserializer, Type type) => new(deserializer, type);
    }
}
