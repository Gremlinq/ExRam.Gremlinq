using System;

namespace ExRam.Gremlinq.Core
{
    public interface IQueryFragmentDeserializer
    {
        object? TryDeserialize<TSerializedData>(TSerializedData serializedData, Type fragmentType, IGremlinQueryEnvironment environment);

        IQueryFragmentDeserializer Override<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, Func<TSerialized, object?>, IQueryFragmentDeserializer, object?> deserializer);
    }
}
