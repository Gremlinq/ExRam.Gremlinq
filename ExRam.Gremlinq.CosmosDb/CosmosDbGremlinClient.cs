using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGremlinClient : IGremlinClient
    {
        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON2Reader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        private readonly IGremlinClient _baseGremlinClient;

        public CosmosDbGremlinClient(GremlinServer server)
        {
            this._baseGremlinClient = new GremlinClient(server, new NullGraphSSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }

        public void Dispose()
        {
            this._baseGremlinClient.Dispose();
        }

        public Task<IReadOnlyCollection<T1>> SubmitAsync<T1>(RequestMessage requestMessage)
        {
            return this._baseGremlinClient.SubmitAsync<T1>(requestMessage);
        }
    }
}