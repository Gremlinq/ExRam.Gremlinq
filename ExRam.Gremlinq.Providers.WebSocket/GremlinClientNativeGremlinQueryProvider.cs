using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class GremlinClientNativeGremlinQueryProvider : ITypedGremlinQueryProvider, IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON3Reader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;

        public GremlinClientNativeGremlinQueryProvider(GremlinServer server) : this(server, NullLogger.Instance)
        {

        }

        public GremlinClientNativeGremlinQueryProvider(GremlinServer server, ILogger logger) : this(
             new GremlinClient(
                server,
                new NullGraphSSON3Reader(),
                new GraphSON3Writer(),
                GremlinClient.DefaultMimeType),
            logger)
        { 
        }

        public GremlinClientNativeGremlinQueryProvider(IGremlinClient client) : this(client, NullLogger.Instance)
        {

        }

        public GremlinClientNativeGremlinQueryProvider(IGremlinClient client, ILogger logger)
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
