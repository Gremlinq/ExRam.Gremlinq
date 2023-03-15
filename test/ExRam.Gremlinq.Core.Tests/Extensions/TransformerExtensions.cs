using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class TransformerExtensions
    {
        public static ITransformer ToGraphsonString(this ITransformer transformer)
        {
            return transformer
                .Add(Create<object, string>((static (data, env, recurse) => new GraphSON2Writer().WriteObject(data))));
        }
    }
}
