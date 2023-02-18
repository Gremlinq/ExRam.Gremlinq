using ExRam.Gremlinq.Core.Deserialization;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class DeserializerExtensions
    {
        public static IDeserializer ToGraphsonString(this IDeserializer deserializer)
        {
            return deserializer
                .Override<object, string>(static (data, env, recurse) => new GraphSON2Writer().WriteObject(data));
        }
    }
}
