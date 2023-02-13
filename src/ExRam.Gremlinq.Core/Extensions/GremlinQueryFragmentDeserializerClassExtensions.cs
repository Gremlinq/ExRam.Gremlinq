using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerClassExtensions
    {
        public readonly struct FluentForClass<TFragmentType>
            where TFragmentType : class
        {
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            public FluentForClass(IGremlinQueryFragmentDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TFragmentType? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TFragmentType>(serialized, environment, out var value)
                ? value
                : default;
        }

        public static FluentForClass<TFragmentType> TryDeserialize<TFragmentType>(this IGremlinQueryFragmentDeserializer deserializer)
            where TFragmentType : class
        {
            return new FluentForClass<TFragmentType>(deserializer);
        }
    }
}
