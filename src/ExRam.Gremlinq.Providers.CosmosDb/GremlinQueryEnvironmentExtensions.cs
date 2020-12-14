using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

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
            private readonly IWebSocketGremlinQueryExecutorBuilder _webSocketBuilder;

            public CosmosDbConfigurationBuilder(IWebSocketGremlinQueryExecutorBuilder webSocketBuilder, string? collectionName = default)
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

            public IGremlinQueryExecutorBuilder ConfigureWebSocket(Func<IWebSocketGremlinQueryExecutorBuilder, IWebSocketGremlinQueryExecutorBuilder> transformation)
            {
                return new CosmosDbConfigurationBuilder(
                    transformation(_webSocketBuilder),
                    _collectionName);
            }

            public IGremlinQueryExecutor Build()
            {
                return _webSocketBuilder.Build();
            }

            public IGremlinQueryEnvironment Environment => _webSocketBuilder.Environment;
        }

        public static IGremlinQueryEnvironment UseCosmosDb(this IGremlinQueryEnvironment env, Func<ICosmosDbConfigurationBuilder, IGremlinQueryExecutorBuilder> transformation)
        {
            return env
                .UseWebSocket(builder => transformation(new CosmosDbConfigurationBuilder(builder.SetSerializationFormat(SerializationFormat.GraphSonV2))))
                .ConfigureFeatureSet(featureSet => featureSet
                    .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                    .ConfigureVariableFeatures(_ => VariableFeatures.BooleanValues | VariableFeatures.IntegerValues | VariableFeatures.ByteValues | VariableFeatures.DoubleValues | VariableFeatures.FloatValues | VariableFeatures.IntegerValues | VariableFeatures.LongValues | VariableFeatures.StringValues)
                    .ConfigureVertexFeatures(_ => VertexFeatures.RemoveVertices | VertexFeatures.MetaProperties | VertexFeatures.AddVertices | VertexFeatures.MultiProperties | VertexFeatures.StringIds | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty)
                    .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.StringIds | VertexPropertyFeatures.UserSuppliedIds | VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                    .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.StringIds | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty)
                    .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.VertexProjectionSteps, ImmutableList<Step>.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, ImmutableList<Step>.Empty))
                .ConfigureSerializer(serializer => serializer
                    .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                        .Override<CosmosDbKey>((key, env, overridden, recurse) => recurse.Serialize(
                            key.PartitionKey != null
                                ? new[] { key.PartitionKey, key.Id }
                                : (object)key.Id,
                            env))
                        .Override<HasKeyStep>((step, env, overridden, recurse) =>
                        {
                            return step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase))
                                ? recurse.Serialize(new WhereTraversalStep(new Step[] {KeyStep.Instance, new IsStep(p)}), env)
                                : overridden(step, env, recurse);
                        })
                        .Override<SkipStep>((step, env, overridden, recurse) => recurse.Serialize(new RangeStep(step.Count, -1, step.Scope), env))
                        .Override<LimitStep>((step, env, overridden, recurse) =>
                        {
                            return step.Count <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");
                        })
                        .Override<TailStep>((step, env, overridden, recurse) =>
                        {
                            return step.Count <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");
                        })
                        .Override<RangeStep>((step, env, overridden, recurse) =>
                        {
                            return step.Lower <= int.MaxValue && step.Upper <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");
                        }))
                    .ToGroovy())
                .StoreTimeSpansAsNumbers();
        }
    }
}
