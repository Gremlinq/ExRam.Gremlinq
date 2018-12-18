using System;
using System.Collections.Generic;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public sealed class CosmosDbGremlinClient : GremlinClient
    {
        private class TimeSpanSerializer : IGraphSONSerializer, IGraphSONDeserializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                TimeSpan value = objectData;
                return GraphSONUtil.ToTypedValue("Double", value.TotalMilliseconds);
            }

            public dynamic Objectify(JToken graphsonObject, GraphSONReader reader)
            {
                var duration = graphsonObject.ToObject<double>();
                return TimeSpan.FromMilliseconds(duration);
            }
        }

        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON2Reader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        public CosmosDbGremlinClient(GremlinServer gremlinServer) : base(
            gremlinServer,
            new NullGraphSSON2Reader(),
            new GraphSON2Writer(new Dictionary<Type, IGraphSONSerializer>
            {
                { typeof(TimeSpan), new TimeSpanSerializer() }
            }),
            GraphSON2MimeType)
        {

        }
    }
}