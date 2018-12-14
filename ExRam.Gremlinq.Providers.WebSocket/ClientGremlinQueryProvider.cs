using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public enum GraphsonVersion
    {
        v2,
        v3
    }

    public class GremlinClientEx : GremlinClient
    {
        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON2Reader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON3Reader : GraphSON3Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        public GremlinClientEx(GremlinServer gremlinServer, GraphsonVersion version) : base(
            gremlinServer,
            version == GraphsonVersion.v2
                ? new NullGraphSSON2Reader()
                : (GraphSONReader)new NullGraphSSON3Reader(),
            version == GraphsonVersion.v2
                ? new GraphSON2Writer()
                : (GraphSONWriter)new GraphSON3Writer(),
            version == GraphsonVersion.v2
                ? GraphSON2MimeType
                : DefaultMimeType)
        {

        }
    }

    public class ClientGremlinQueryProvider : IGremlinQueryProvider, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;

        public ClientGremlinQueryProvider(IGremlinClient client) : this(client, NullLogger.Instance)
        {

        }

        public ClientGremlinQueryProvider(IGremlinClient client, ILogger logger)
        {
            _logger = logger;
            _gremlinClient = client;
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            if (typeof(TElement) != typeof(JToken))
                throw new NotSupportedException();

            var serialized = query
                .Serialize();

            _logger.LogTrace("Executing Gremlin query {0}.", serialized.queryString);

            return _gremlinClient
                .SubmitAsync<JToken>(serialized.queryString, new Dictionary<string, object>(serialized.parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x
                    .ToAsyncEnumerable()
                    .Select(y => (TElement)(object)y));
        }
    }
}
