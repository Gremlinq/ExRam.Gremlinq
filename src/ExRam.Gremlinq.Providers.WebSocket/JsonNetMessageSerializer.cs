﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class JsonNetMessageSerializer : IMessageSerializer
    {
        private sealed class GraphSON2JsonNetMessageSerializer : JsonNetMessageSerializer
        {
            public GraphSON2JsonNetMessageSerializer() : base("application/vnd.gremlin-v2.0+json", new GraphSON2Writer())
            {

            }
        }

        private sealed class GraphSON3JsonNetMessageSerializer : JsonNetMessageSerializer
        {
            public GraphSON3JsonNetMessageSerializer() : base("application/vnd.gremlin-v3.0+json", new GraphSON3Writer())
            {

            }
        }

        public static readonly IMessageSerializer GraphSON2 = new GraphSON2JsonNetMessageSerializer();
        public static readonly IMessageSerializer GraphSON3 = new GraphSON3JsonNetMessageSerializer();

        private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault();

        private readonly string _mimeType;
        private readonly GraphSONWriter _graphSONWriter;

        protected JsonNetMessageSerializer(string mimeType, GraphSONWriter graphSonWriter)
        {
            _mimeType = mimeType;
            _graphSONWriter = graphSonWriter;
        }

        public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage)
        {
            var graphSONMessage = _graphSONWriter.WriteObject(requestMessage);
            return Encoding.UTF8.GetBytes(MessageWithHeader(graphSONMessage));
        }

        private string MessageWithHeader(string messageContent)
        {
            return $"{(char)_mimeType.Length}{_mimeType}{messageContent}";
        }

        public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message)
        {
            if (message.Length == 0)
                return null!;

            var responseMessage = Serializer
                .Deserialize<ResponseMessage<JToken>>(new JsonTextReader(new StreamReader(new MemoryStream(message))));

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
    }
}
