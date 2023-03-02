﻿using ExRam.Gremlinq.Core.Serialization;
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
                        .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                        .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                        .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty))
                    .ConfigureSerializer(serializer => serializer
                        .Add<byte[]>((bytes, env, recurse) => recurse.Serialize(Convert.ToBase64String(bytes), env))
                        .Add<CosmosDbKey>((key, env, recurse) => recurse.Serialize(
                            key.PartitionKey != null
                                ? new[] { key.PartitionKey, key.Id }
                                : (object)key.Id,
                            env))
                        .Add<FilterStep.ByTraversalStep>(static (step, env, recurse) => recurse.Serialize(
                            new WhereTraversalStep(
                                step.Traversal.Count > 0 && step.Traversal[0] is AsStep
                                    ? new MapStep(step.Traversal)
                                    : step.Traversal),
                            env))
                        .Add<HasKeyStep>((step, env, recurse) => step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase))
                            ? recurse.Serialize(
                                new WhereTraversalStep(Traversal.Empty.Push(
                                    KeyStep.Instance,
                                    new IsStep(p))),
                                env)
                            : default)
                        .Add<NoneStep>((step, env, recurse) => recurse.Serialize(NoneWorkaround, env))
                        .Add<SkipStep>((step, env, recurse) => recurse.Serialize(new RangeStep(step.Count, -1, step.Scope), env))
                        .Add<LimitStep>((step, env, recurse) => step.Count > int.MaxValue
                            ? throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.")
                            : default)
                        .Add<TailStep>((step, env, recurse) => step.Count > int.MaxValue
                            ? throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.")
                            : default)
                        .Add<RangeStep>((step, env, recurse) => step.Lower > int.MaxValue || step.Upper > int.MaxValue
                            ? throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.")
                            : default)
                        .Add<Order>((order, env, recurse) => order.Equals(Order.Asc)
                            ? recurse.Serialize(WorkaroundOrder.Incr, env)
                            : order.Equals(Order.Desc)
                                ? recurse.Serialize(WorkaroundOrder.Decr, env)
                                : default)
                        .ToGroovy())
                    .StoreTimeSpansAsNumbers());
        }
    }
}
