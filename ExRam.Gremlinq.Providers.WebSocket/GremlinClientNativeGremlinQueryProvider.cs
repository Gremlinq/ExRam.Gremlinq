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
    public class GremlinClientNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>, IDisposable
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

        public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
        {
            _logger.LogTrace("Executing Gremlin query {0}.", query);

            return _gremlinClient
                .SubmitAsync<JToken>(query, new Dictionary<string, object>(parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x.ToAsyncEnumerable());
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }
    }
}
