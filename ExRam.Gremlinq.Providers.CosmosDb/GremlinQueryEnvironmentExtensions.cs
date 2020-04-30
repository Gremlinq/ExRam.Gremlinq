using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Providers.WebSocket;
using Newtonsoft.Json.Linq;

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

        public static IGremlinQueryEnvironment UseCosmosDb(this IGremlinQueryEnvironment env, Func<ICosmosDbConfigurationBuilder, ICosmosDbConfigurationBuilderWithAuthKey> transformation)
        {
            return env
                .UseWebSocket(builder => transformation(new CosmosDbConfigurationBuilder(builder.SetGraphSONVersion(GraphsonVersion.V2))))
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
                    .SetValue(GremlinqOption.VertexProjectionSteps, ImmutableList<Step>.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, ImmutableList<Step>.Empty))
                .ConfigureDeserializer(deserializer => deserializer
                    .ConfigureFragmentDeserializer(fragmentDeserializer => fragmentDeserializer
                        .Override<JToken>((jToken, type, env, overridden, recurse) =>
                        {
                            if (type == typeof(TimeSpan))
                            {
                                if (recurse.TryDeserialize(jToken, typeof(double), env) is double value)
                                {
                                    return TimeSpan.FromMilliseconds(value);
                                }
                            }

                            return overridden(jToken);
                        })));
        }
    }
}
