using System.Collections.Concurrent;
using System.Reflection;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerTypeExtensions
    {
        public readonly struct FluentForType
        {
            private readonly Type _type;
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromClassDelegates = new();
            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromStructDelegates = new();

            public FluentForType(IGremlinQueryFragmentDeserializer deserializer, Type type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = type.GetGenericArguments()[0];

                _type = type;
                _deserializer = deserializer;
            }

            public object? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment)
            {
                var delegatesDict = _type.IsValueType
                    ? FromStructDelegates
                    : FromClassDelegates;

                var maybeFromDelegate = delegatesDict
                    .GetOrAdd(
                        (typeof(TSerialized), _type),
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

                return maybeFromDelegate is Func<IGremlinQueryFragmentDeserializer, TSerialized, IGremlinQueryEnvironment, object?> fromDelegate
                    ? fromDelegate(_deserializer, serialized, environment)
                    : default;
            }

            private static Func<IGremlinQueryFragmentDeserializer, TSerialized, IGremlinQueryEnvironment, object?> FromClass<TSerialized, TFragment>()
                where TFragment : class => (deserializer, serialized, environment) => deserializer.TryDeserialize<TFragment>().From(serialized, environment);

            private static Func<IGremlinQueryFragmentDeserializer, TSerialized, IGremlinQueryEnvironment, object?> FromStruct<TSerialized, TFragment>()
                where TFragment : struct => (deserializer, serialized, environment) => deserializer.TryDeserialize<TFragment>().From(serialized, environment);
        }

        public static FluentForType TryDeserialize(this IGremlinQueryFragmentDeserializer deserializer, Type type)
        {
            return new FluentForType(deserializer, type);
        }
    }
}
