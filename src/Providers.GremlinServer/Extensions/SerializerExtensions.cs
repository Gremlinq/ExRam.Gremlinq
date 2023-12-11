using System.Buffers;
using System.Text;

using CommunityToolkit.HighPerformance.Buffers;

using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core
{
    internal static class SerializerExtensions
    {
        public static IGremlinQueryEnvironment UseGraphSon2(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON2Writer(), "application/vnd.gremlin-v2.0+json", owner => new GraphSon2MessageBuffer(owner));

        public static IGremlinQueryEnvironment UseGraphSon3(this IGremlinQueryEnvironment environment) => environment.UseGraphSon(new GraphSON3Writer(), "application/vnd.gremlin-v3.0+json", owner => new GraphSon3MessageBuffer(owner));

        private static IGremlinQueryEnvironment UseGraphSon<TBuffer>(this IGremlinQueryEnvironment environment, GraphSONWriter writer, string mimeType, Func<IMemoryOwner<byte>, TBuffer> bufferFactory)
            where TBuffer : struct, IMemoryOwner<byte>
        {
            var mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}");

            return environment
                .ConfigureSerializer(serializer => serializer
                    .Add(Create<RequestMessage, TBuffer>((message, _, _, _) =>
                    {
                        var graphSONMessage = writer.WriteObject(message);
                        var bytesNeeded = Encoding.UTF8.GetByteCount(graphSONMessage) + mimeTypeBytes.Length;
                        var memory = MemoryOwner<byte>.Allocate(bytesNeeded);

                        mimeTypeBytes
                            .AsSpan()
                            .CopyTo(memory.Memory.Span);

                        Encoding.UTF8.GetBytes(graphSONMessage.AsSpan(), memory.Memory.Span[mimeTypeBytes.Length..]);

                        return bufferFactory(memory);
                    })));
        }
    }
}
