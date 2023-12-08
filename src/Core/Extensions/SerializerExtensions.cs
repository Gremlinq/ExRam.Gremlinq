using System.Buffers;
using System.Text;

using CommunityToolkit.HighPerformance.Buffers;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core
{
    public static class SerializerExtensions
    {
        public static IGremlinQueryEnvironment UseGraphSon2(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON2Writer(), "application/vnd.gremlin-v2.0+json");

        public static IGremlinQueryEnvironment UseGraphSon3(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON3Writer(), "application/vnd.gremlin-v3.0+json");

        private static IGremlinQueryEnvironment UseGraphSon(this IGremlinQueryEnvironment environment, GraphSONWriter writer, string mimeType)
        {
            var mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}");

            return environment
                .ConfigureSerializer(serializer => serializer
                    .Add(Create<RequestMessage, IMemoryOwner<byte>>((message, _, _, _) =>
                    {
                        var graphSONMessage = writer.WriteObject(message);
                        var bytesNeeded = Encoding.UTF8.GetByteCount(graphSONMessage) + mimeTypeBytes.Length;
                        var memory = MemoryOwner<byte>.Allocate(bytesNeeded);

                        mimeTypeBytes
                            .AsSpan()
                            .CopyTo(memory.Memory.Span);

                        Encoding.UTF8.GetBytes(graphSONMessage.AsSpan(), memory.Memory.Span[mimeTypeBytes.Length..]);

                        return memory;
                    })));
        }
    }
}
