using System;
using System.Collections.Generic;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class TimeSpanSerializer : IGraphSONSerializer, IGraphSONDeserializer
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

        private sealed class TimespanConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TimeSpan);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return TimeSpan.FromMilliseconds(serializer.Deserialize<long>(reader));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        public static IGremlinQueryEnvironment UseCosmosDb(this IGremlinQueryEnvironment env, Uri uri, string database, string graphName, string authKey)
        {
            return env
                .UseWebSocket(builder => builder
                    .WithUri(uri)
                    .WithAuthentication($"/dbs/{database}/colls/{graphName}", authKey)
                    .WithGraphSONVersion(GraphsonVersion.V2)
                    .WithGraphSONSerializer(typeof(TimeSpan), new TimeSpanSerializer()))
                .ConfigureExecutionPipeline(pipeline => pipeline
                    .ConfigureSerializer(serializer => serializer
                        .UseCosmosDbWorkarounds()
                        .ToGroovy())
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.GraphsonWithJsonConverters(new TimespanConverter())));
        }
    }
}
