using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Structure.IO.GraphSON;
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

            public ICosmosDbConfigurationBuilder ConfigureWebSocket(Func<IWebSocketConfigurationBuilder, IWebSocketConfigurationBuilder> transformation)
            {
                return new CosmosDbConfigurationBuilder(
                    transformation(_webSocketBuilder),
                    _collectionName);
            }

            public IGremlinQueryEnvironment Build()
            {
                return _webSocketBuilder.Build();
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

        private sealed class TimespanConverter : IJTokenConverter
        {
            public bool TryConvert(JToken jToken, Type objectType, IJTokenConverter recurse, [AllowNull] out object? value)
            {
                if (objectType == typeof(TimeSpan))
                {
                    if (recurse.TryConvert(jToken, typeof(long), recurse, out value))
                    {
                        value = TimeSpan.FromMilliseconds((long)value);
                        return true;
                    }
                }

                value = null;
                return false;
            }
        }

        public static IGremlinQueryEnvironment UseCosmosDb(this IGremlinQueryEnvironment env, Func<ICosmosDbConfigurationBuilder, ICosmosDbConfigurationBuilderWithAuthKey> transformation)
        {
            return env
                .UseWebSocket(builder =>
                    transformation(
                        new CosmosDbConfigurationBuilder(
                            builder
                                .SetGraphSONVersion(GraphsonVersion.V2)
                                .AddGraphSONSerializer(typeof(TimeSpan), new TimeSpanSerializer()))))
                .ConfigureFeatureSet(featureSet => featureSet
                    .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                    .ConfigureVariableFeatures(_ => VariableFeatures.BooleanValues | VariableFeatures.IntegerValues | VariableFeatures.ByteValues | VariableFeatures.DoubleValues | VariableFeatures.FloatValues | VariableFeatures.IntegerValues | VariableFeatures.LongValues | VariableFeatures.StringValues)
                    .ConfigureVertexFeatures(_ => VertexFeatures.RemoveVertices | VertexFeatures.MetaProperties | VertexFeatures.AddVertices | VertexFeatures.MultiProperties | VertexFeatures.StringIds | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty)
                    .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.StringIds | VertexPropertyFeatures.UserSuppliedIds | VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                    .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.StringIds | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty)
                    .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                .ConfigureSerializer(serializer => serializer
                    .UseCosmosDbWorkarounds()
                    .ToGroovy())
                .ConfigureOptions(options => options
                    .SetItem(GremlinqOption.DontAddElementProjectionSteps, true))
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.GraphsonWithJsonConverters(new TimespanConverter()));
        }
    }
}
