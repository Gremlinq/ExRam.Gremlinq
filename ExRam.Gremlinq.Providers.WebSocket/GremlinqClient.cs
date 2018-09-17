using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class GremlinqClient : IGremlinClient
    {
        private class TimeSpanSerializer : IGraphSONSerializer, IGraphSONDeserializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                TimeSpan value = objectData;
                return GraphSONUtil.ToTypedValue("Double", value.TotalSeconds);
            }

            public dynamic Objectify(JToken graphsonObject, GraphSONReader reader)
            {
                var duration = graphsonObject.ToObject<double>();
                return TimeSpan.FromSeconds(duration);
            }
        }

        internal class DateSerializer : IGraphSONSerializer, IGraphSONDeserializer
        {
            private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                DateTime value = objectData;
                var ticks = (value.ToUniversalTime() - UnixStart).Ticks;
                return GraphSONUtil.ToTypedValue("Date", ticks / TimeSpan.TicksPerMillisecond);
            }

            public dynamic Objectify(JToken graphsonObject, GraphSONReader reader)
            {
                var milliseconds = graphsonObject.ToObject<long>();
                return UnixStart.AddTicks(TimeSpan.TicksPerMillisecond * milliseconds);
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

        private readonly IGremlinClient _baseGremlinClient;

        public GremlinqClient(GremlinServer server)
        {
            this._baseGremlinClient = new GremlinClient(
                server,
                new NullGraphSSON2Reader(),
                new GraphSON2Writer(new Dictionary<Type, IGraphSONSerializer>
                {
                    { typeof(TimeSpan), new TimeSpanSerializer() } ,
                    { typeof(DateTime), new DateSerializer() }
                }),
                GremlinClient.GraphSON2MimeType);
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