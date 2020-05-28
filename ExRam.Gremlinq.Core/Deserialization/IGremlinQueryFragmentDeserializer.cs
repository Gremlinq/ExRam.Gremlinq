using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryFragmentDeserializer
    {
        object? TryDeserialize<TSerializedData>(TSerializedData serializedData, Type fragmentType, IGremlinQueryEnvironment environment);

        IGremlinQueryFragmentDeserializer Override<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?>, IGremlinQueryFragmentDeserializer, object?> deserializer);
    }
}
