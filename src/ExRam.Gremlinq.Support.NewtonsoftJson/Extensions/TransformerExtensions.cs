using System.Text;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Transformation
{
    public static class TransformerExtensions
    {
        public static ITransformer UseGraphSon2(this ITransformer transformer) => transformer.UseGraphSon(new GraphSON2Writer(), "application/vnd.gremlin-v2.0+json");

        public static ITransformer UseGraphSon3(this ITransformer transformer) => transformer.UseGraphSon(new GraphSON3Writer(), "application/vnd.gremlin-v3.0+json");

        private static ITransformer UseGraphSon(this ITransformer transformer, GraphSONWriter writer, string mimeType)
        {
            var mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}");

            return transformer
                .Add(ConverterFactory.Create<RequestMessage, byte[]>((message, env, recurse) =>
                {
                    var graphSONMessage = writer.WriteObject(message);
                    var ret = new byte[Encoding.UTF8.GetByteCount(graphSONMessage) + mimeTypeBytes.Length];

                    mimeTypeBytes.CopyTo(ret, 0);
                    Encoding.UTF8.GetBytes(graphSONMessage, 0, graphSONMessage.Length, ret, mimeTypeBytes.Length);

                    return ret;
                }));
        }
    }
}
