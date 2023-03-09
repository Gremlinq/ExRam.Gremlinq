using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    internal sealed class DefaultMessageSerializer : IMessageSerializer
    {
        private readonly IGremlinQueryEnvironment _environment;

        public DefaultMessageSerializer(IGremlinQueryEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage) => _environment.Serializer
            .TransformTo<byte[]>()
            .From(requestMessage, _environment);

        public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message) => message.Length == 0
            ? null!
            : _environment.Deserializer
                .TransformTo<ResponseMessage<List<object>>>()
                .From(message, _environment);
    }
}
