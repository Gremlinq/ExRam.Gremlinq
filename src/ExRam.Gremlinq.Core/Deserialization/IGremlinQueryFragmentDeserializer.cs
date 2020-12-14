using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryFragmentDeserializer
    {
        object? TryDeserialize<TSerializedData>(TSerializedData serializedData, Type fragmentType, IGremlinQueryEnvironment environment);

        IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer);
    }
}
