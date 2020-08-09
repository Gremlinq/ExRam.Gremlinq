using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IEnvironmentCache
    {
        JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);

        string GetVertexLabel(Type type);
        string GetEdgeLabel(Type type);
        (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type);
            
        IReadOnlyDictionary<string, Type[]> ModelTypes { get; }
    }
}
