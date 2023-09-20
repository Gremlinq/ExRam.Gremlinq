using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class DefaultMessageSerializer : IMessageSerializer
    {
        private readonly IGremlinQueryEnvironment _environment;

        public DefaultMessageSerializer(IGremlinQueryEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage, CancellationToken ct) => _environment.Serializer
            .TransformTo<byte[]>()
            .From(requestMessage, _environment);

        public async Task<ResponseMessage<List<object>>?> DeserializeMessageAsync(byte[] message, CancellationToken ct) => message.Length == 0
            ? null
            : _environment.Deserializer.TryTransformTo<ResponseMessage<List<object>>>().From(message, _environment);
    }
}
