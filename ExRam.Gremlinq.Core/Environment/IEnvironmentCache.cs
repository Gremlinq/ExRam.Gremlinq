using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IEnvironmentCache
    {
        JsonSerializer GetPopulatingJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);
        JsonSerializer GetIgnoringJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);

        string GetVertexLabel(Type type);
        string GetEdgeLabel(Type type);

        IReadOnlyDictionary<string, Type[]> ModelTypes { get; }
    }
}
