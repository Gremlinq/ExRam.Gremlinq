using ExRam.Gremlinq.Core.Deserialization;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryFragmentDeserializerExtensions
    {
        public static IGremlinQueryFragmentDeserializer ToGraphsonString(this IGremlinQueryFragmentDeserializer deserializer)
        {
            return deserializer
                .Override<object, string>(static (data, env, recurse) => new GraphSON2Writer().WriteObject(data));
        }
    }
}
