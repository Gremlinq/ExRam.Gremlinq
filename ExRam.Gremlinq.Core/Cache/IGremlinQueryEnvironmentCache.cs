using System;
using System.Collections.Generic;
using System.Reflection;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IGremlinQueryEnvironmentCache
    {
        JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        Key GetKey(MemberInfo member);
        (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type);

        IReadOnlyDictionary<Type, object?> FastNativeTypes { get;  }
        IReadOnlyDictionary<string, Type[]> ModelTypes { get; }
    }
}
