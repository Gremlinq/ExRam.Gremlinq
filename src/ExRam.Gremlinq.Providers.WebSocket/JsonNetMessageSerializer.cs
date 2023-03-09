using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core
{
    internal sealed class JsonNetMessageSerializer : IMessageSerializer
    {
        private readonly IGremlinQueryEnvironment _environment;

        public JsonNetMessageSerializer(IGremlinQueryEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage)
        {
            return _environment.Serializer
                .TransformTo<byte[]>()
                .From(requestMessage, _environment);
        }

        public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message)
        {
            if (message.Length == 0)
                return null!;

            return _environment.Deserializer
                .TransformTo<ResponseMessage<List<object>>>()
                .From(message, _environment);
        }
    }
}
