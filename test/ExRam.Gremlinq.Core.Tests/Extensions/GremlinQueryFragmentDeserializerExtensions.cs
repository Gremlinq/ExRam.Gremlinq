using ExRam.Gremlinq.Core.Deserialization;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryFragmentDeserializerExtensions
    {
        public static IGremlinQueryFragmentDeserializer ToGraphsonString(this IGremlinQueryFragmentDeserializer deserializer)
        {
            return deserializer
                .Override<object>(static (data, type, env, overridden, recurse) => type.IsAssignableFrom(typeof(string))
                    ? new GraphSON2Writer().WriteObject(data)
                    : overridden(data, type, env, recurse));
        }
    }
}
