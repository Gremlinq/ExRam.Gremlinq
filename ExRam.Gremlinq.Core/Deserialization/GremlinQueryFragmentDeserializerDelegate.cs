using System;

namespace ExRam.Gremlinq.Core
{
    public delegate object? GremlinQueryFragmentDeserializerDelegate<TSerialized>(TSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> overridden, IGremlinQueryFragmentDeserializer recurse);
}
