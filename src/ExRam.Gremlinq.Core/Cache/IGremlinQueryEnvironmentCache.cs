using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IGremlinQueryEnvironmentCache
    {
        JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        Key GetKey(MemberInfo member);
        (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type);
        QuerySemantics? TryGetQuerySemanticsFromElementType(Type type);  

        HashSet<Type> ModelTypes { get; }
        IReadOnlyDictionary<Type, object?> FastNativeTypes { get;  }
        IReadOnlyDictionary<string, Type[]> ModelTypesForLabels { get; }
    }
}
