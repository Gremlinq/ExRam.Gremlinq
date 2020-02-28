using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class CosmosDbConfigurationBuilder :
            ICosmosDbConfigurationBuilder,
            ICosmosDbConfigurationBuilderWithUri,
            ICosmosDbConfigurationBuilderWithAuthKey
        {
            private readonly string? _collectionName;
            private readonly IWebSocketConfigurationBuilder _webSocketBuilder;

            public CosmosDbConfigurationBuilder(IWebSocketConfigurationBuilder webSocketBuilder, string? collectionName = default)
            {
                _collectionName = collectionName;
                _webSocketBuilder = webSocketBuilder;
            }

            public ICosmosDbConfigurationBuilderWithUri At(Uri uri, string databaseName, string graphName)
            {
                return new CosmosDbConfigurationBuilder(_webSocketBuilder.At(uri), $"/dbs/{databaseName}/colls/{graphName}");
            }

            public ICosmosDbConfigurationBuilderWithAuthKey AuthenticateBy(string authKey)
            {
                return new CosmosDbConfigurationBuilder(
                    _webSocketBuilder.AuthenticateBy(_collectionName!, authKey),
                    _collectionName);
            }

            public ICosmosDbConfigurationBuilderWithAuthKey ConfigureWebSocket(Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation)
            {
                return new CosmosDbConfigurationBuilder(
                    transformation(_webSocketBuilder),
                    _collectionName);
            }

            public IWebSocketConfigurationBuilder Build()
            {
                return _webSocketBuilder;
            }
        }

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

        public static IGremlinQueryEnvironment UseCosmosDb(this IGremlinQueryEnvironment env, Func<ICosmosDbConfigurationBuilder, ICosmosDbConfigurationBuilderWithAuthKey> transformation)
        {
            return env
                .UseWebSocket(builder =>
                    transformation(
                        new CosmosDbConfigurationBuilder(
                            builder
                                .SetGraphSONVersion(GraphsonVersion.V2)
                                .AddGraphSONSerializer(typeof(TimeSpan), new TimeSpanSerializer())))
                    .Build())
                .ConfigureSerializer(serializer => serializer
                    .UseCosmosDbWorkarounds()
                    .ToGroovy())
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.GraphsonWithJsonConverters(new TimespanConverter()));
        }
    }
}
