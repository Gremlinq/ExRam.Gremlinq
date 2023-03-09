using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal sealed class JsonNetMessageSerializer : IMessageSerializer
    {
        private static readonly JsonSerializer Serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });
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

            var maybeResponseMessage = Serializer
                .Deserialize<ResponseMessage<JToken>>(new JsonTextReader(new StreamReader(new MemoryStream(message))));

            if (maybeResponseMessage is { } responseMessage)
            {
                return new ResponseMessage<List<object>>
                {
                    RequestId = responseMessage.RequestId,
                    Status = responseMessage.Status,
                    Result = new ResponseResult<List<object>>
                    {
                        Data = new List<object>
                        {
                            responseMessage.Result.Data
                        },
                        Meta = responseMessage.Result.Meta
                    }
                };
            }

            throw new InvalidDataException($"Unable to deserialize the data into a {nameof(ResponseMessage<JToken>)}.");
        }
    }
}
