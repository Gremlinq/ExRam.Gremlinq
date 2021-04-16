using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class CosmosDbConfigurator : ICosmosDbConfigurator
        {
            private readonly Uri? _uri;
            private readonly string? _authKey;
            private readonly string? _collectionName;
            private readonly IWebSocketConfigurator _webSocketBuilder;

            public CosmosDbConfigurator(IWebSocketConfigurator webSocketBuilder, Uri? uri, string? collectionName, string? authKey)
            {
                _authKey = authKey;
                _collectionName = collectionName;
                _webSocketBuilder = webSocketBuilder;
                _uri = uri;
            }

            public ICosmosDbConfigurator At(Uri uri, string databaseName, string graphName)
            {
                return new CosmosDbConfigurator(
                    _webSocketBuilder,
                    uri,
                    $"/dbs/{databaseName}/colls/{graphName}",
                    _authKey);
            }

            public ICosmosDbConfigurator AuthenticateBy(string authKey)
            {
                return new CosmosDbConfigurator(
                    _webSocketBuilder,
                    _uri,
                    _collectionName,
                    authKey);
            }

            public ICosmosDbConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation)
            {
                return new CosmosDbConfigurator(
                    transformation(_webSocketBuilder),
                    _uri,
                    _collectionName,
                    _authKey);
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                if (_uri is { } uri && _authKey is { } authKey && _collectionName is { } collectionName)
                {
                    return _webSocketBuilder
                        .At(uri)
                        .AuthenticateBy(collectionName, authKey)
                        .SetSerializationFormat(SerializationFormat.GraphSonV2)
                        .Transform(source);
                }

                throw new InvalidOperationException($"The {nameof(ICosmosDbConfigurator)} is not properly configured.");
            }
        }

        public static IGremlinQuerySource UseCosmosDb(this IConfigurableGremlinQuerySource source, Func<ICosmosDbConfigurator, IGremlinQuerySourceTransformation> transformation)
        {
            return source
                .UseWebSocket(builder => transformation(new CosmosDbConfigurator(builder, null, null, null)))
                .ConfigureEnvironment(environment => environment
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
                            .Override<byte[]>((bytes, env, overridden, recurse) => recurse.Serialize(Convert.ToBase64String(bytes), env))
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
                    .StoreTimeSpansAsNumbers());
        }
    }
}
