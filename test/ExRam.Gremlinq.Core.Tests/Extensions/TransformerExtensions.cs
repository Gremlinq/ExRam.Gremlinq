using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class TransformerExtensions
    {
        public static ITransformer ToGraphsonString(this ITransformer transformer)
        {
            return transformer
                .Override<object, string>(static (data, env, recurse) => new GraphSON2Writer().WriteObject(data));
        }
    }
}
