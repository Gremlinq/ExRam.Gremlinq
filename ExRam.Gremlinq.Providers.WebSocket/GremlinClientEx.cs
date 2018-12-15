using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
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
            version == GraphsonVersion.V2
                ? new NullGraphSSON2Reader()
                : (GraphSONReader)new NullGraphSSON3Reader(),
            version == GraphsonVersion.V2
                ? new GraphSON2Writer()
                : (GraphSONWriter)new GraphSON3Writer(),
            version == GraphsonVersion.V2
                ? GraphSON2MimeType
                : DefaultMimeType)
        {

        }
    }
}
