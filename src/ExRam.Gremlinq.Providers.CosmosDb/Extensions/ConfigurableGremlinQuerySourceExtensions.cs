using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class CosmosDbConfigurator : ICosmosDbConfigurator
        {
            private readonly string? _authKey;
            private readonly string? _graphName;
            private readonly string? _databaseName;
            private readonly IWebSocketConfigurator _webSocketConfigurator;

            public CosmosDbConfigurator(IWebSocketConfigurator webSocketConfigurator, string? databaseName, string? graphName, string? authKey)
            {
                _authKey = authKey;
                _graphName = graphName;
                _databaseName = databaseName;
                _webSocketConfigurator = webSocketConfigurator;
            }

            public ICosmosDbConfigurator OnDatabase(string databaseName) => new CosmosDbConfigurator(
                _webSocketConfigurator,
                databaseName,
                _graphName,
                _authKey);

            public ICosmosDbConfigurator OnGraph(string graphName) => new CosmosDbConfigurator(
                _webSocketConfigurator,
                _databaseName,
                graphName,
                _authKey);

            public ICosmosDbConfigurator AuthenticateBy(string authKey) => new CosmosDbConfigurator(
                _webSocketConfigurator,
                _databaseName,
                _graphName,
                authKey);

            public ICosmosDbConfigurator ConfigureWebSocket(Func<IWebSocketConfigurator, IWebSocketConfigurator> transformation) => new CosmosDbConfigurator(
                transformation(_webSocketConfigurator),
                _databaseName,
                _graphName,
                _authKey);

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                var webSocketConfigurator = _webSocketConfigurator
                    .ConfigureMessageSerializer(_ => JsonNetMessageSerializer.GraphSON2);

                if (_databaseName is { } databaseName && _graphName is { } graphName && _authKey is { } authKey)
                    webSocketConfigurator = webSocketConfigurator.AuthenticateBy($"/dbs/{databaseName}/colls/{graphName}", authKey);

                return webSocketConfigurator
                    .Transform(source);
            }
        }

        private class WorkaroundOrder : EnumWrapper, IComparator
        {
            public static readonly WorkaroundOrder Incr = new("incr");
            public static readonly WorkaroundOrder Decr = new("decr");

            private WorkaroundOrder(string enumValue)  : base("Order", enumValue)
            {
            }
        }

        private static readonly Traversal VertexProjectionSteps = new Step[]
        {
            new MapStep(
                new Step[]
                {
                    new UnionStep(
                        new Traversal[]
                        {
                            new Step[]
                            {
                                new ProjectStep(ImmutableArray.Create("id", "label")),
                                new ProjectStep.ByKeyStep(T.Id),
                                new ProjectStep.ByKeyStep(T.Label),
                                UnfoldStep.Instance
                            },
                            new Step[]
                            {
                                new PropertiesStep(ImmutableArray<string>.Empty),
                                new HasPredicateStep(T.Key, P.Neq("id").And(P.Neq("label"))),
                                GroupStep.Instance,
                                new GroupStep.ByKeyStep(T.Label),
                                new GroupStep.ByTraversalStep(new Step[]
                                {
                                    new ProjectStep(ImmutableArray.Create("id", "label", "value", "properties")),
                                    new ProjectStep.ByKeyStep(T.Id),
                                    new ProjectStep.ByKeyStep(T.Label),
                                    new ProjectStep.ByTraversalStep(ValueStep.Instance),
                                    new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty)),
                                    FoldStep.Instance
                                }),
                                UnfoldStep.Instance
                            }
                        }
                        .ToImmutableArray()),
                    GroupStep.Instance,
                    new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Keys)),
                    new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Values))
                })
        };

        private static readonly Traversal VertexPropertyProjectionSteps = new Step[]
        {
            new ProjectStep(ImmutableArray.Create("id", "label", "value", "properties")),
            new ProjectStep.ByKeyStep(T.Id),
            new ProjectStep.ByKeyStep(T.Label),
            new ProjectStep.ByTraversalStep(ValueStep.Instance),
            new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty)),
        };

        private static readonly Traversal EdgeProjectionSteps = new Step[]
        {
            new MapStep(
                new Step[]
                {
                    new UnionStep(
                        new Traversal[]
                        {
                            new Step[]
                            {
                                new ProjectStep(ImmutableArray.Create("id", "label")),
                                new ProjectStep.ByKeyStep(T.Id),
                                new ProjectStep.ByKeyStep(T.Label),
                                UnfoldStep.Instance
                            },
                            new Step[]
                            {
                                new PropertiesStep(ImmutableArray<string>.Empty),
                                new HasPredicateStep(T.Key, P.Neq("id").And(P.Neq("label"))),
                                GroupStep.Instance,
                                new GroupStep.ByKeyStep(T.Key),
                                new GroupStep.ByTraversalStep(new ValueStep()),
                                UnfoldStep.Instance
                            }
                        }
                        .ToImmutableArray()),
                    GroupStep.Instance,
                    new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Keys)),
                    new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Values))
                })
        };

        private static readonly Step NoneWorkaround = new NotStep(IdentityStep.Instance);

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
                        .SetValue(GremlinqOption.VertexProjectionSteps, VertexProjectionSteps)
                        .SetValue(GremlinqOption.EdgeProjectionSteps, EdgeProjectionSteps)
                        .SetValue(GremlinqOption.VertexPropertyProjectionSteps, VertexPropertyProjectionSteps))
                    .ConfigureSerializer(serializer => serializer
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<byte[]>((bytes, env, overridden, recurse) => recurse.Serialize(Convert.ToBase64String(bytes), env))
                            .Override<CosmosDbKey>((key, env, overridden, recurse) => recurse.Serialize(
                                key.PartitionKey != null
                                    ? new[] { key.PartitionKey, key.Id }
                                    : (object)key.Id,
                                env))
                            .Override<FilterStep.ByTraversalStep>(static (step, env, _, recurse) => recurse.Serialize(
                                new WhereTraversalStep(
                                    step.Traversal.Count > 0 && step.Traversal[0] is AsStep
                                        ? new MapStep(step.Traversal)
                                        : step.Traversal),
                                env))
                            .Override<HasKeyStep>((step, env, overridden, recurse) => step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase))
                                ? recurse.Serialize(new WhereTraversalStep(new Step[] {KeyStep.Instance, new IsStep(p)}), env)
                                : overridden(step, env, recurse))
                            .Override<NoneStep>((step, env, overridden, recurse) => recurse.Serialize(NoneWorkaround, env))
                            .Override<SkipStep>((step, env, overridden, recurse) => recurse.Serialize(new RangeStep(step.Count, -1, step.Scope), env))
                            .Override<LimitStep>((step, env, overridden, recurse) => step.Count <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer."))
                            .Override<TailStep>((step, env, overridden, recurse) => step.Count <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer."))
                            .Override<RangeStep>((step, env, overridden, recurse) => step.Lower <= int.MaxValue && step.Upper <= int.MaxValue
                                ? overridden(step, env, recurse)
                                : throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer."))
                            .Override<Order>((order, env, overridden, recurse) => order.Equals(Order.Asc)
                                ? recurse.Serialize(WorkaroundOrder.Incr, env)
                                : order.Equals(Order.Desc)
                                    ? recurse.Serialize(WorkaroundOrder.Decr, env)
                                    : overridden(order, env, recurse)))
                        .ToGroovy())
                    .StoreTimeSpansAsNumbers());
        }
    }
}
