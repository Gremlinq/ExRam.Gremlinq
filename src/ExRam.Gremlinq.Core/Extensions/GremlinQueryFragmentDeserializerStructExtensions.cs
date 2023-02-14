using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerStructExtensions
    {
        public readonly struct FluentForStruct<TFragmentType>
            where TFragmentType : struct
        {
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            public FluentForStruct(IGremlinQueryFragmentDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TFragmentType? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TFragmentType>(serialized, environment, out var value)
                ? value
                : default(TFragmentType?);
        }

        public static FluentForStruct<TFragmentType> TryDeserialize<TFragmentType>(this IGremlinQueryFragmentDeserializer deserializer)
            where TFragmentType : struct
        {
            return new FluentForStruct<TFragmentType>(deserializer);
        }

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TNative>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, Func<TSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TNative?> func)
            where TNative : struct
        {
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, recurse) => type == typeof(TNative)
                    ? func(token, env, recurse) is { } value
                        ? value
                        : default(object?)
                    : default);
        }
    }
}
