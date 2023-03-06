using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

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

        private sealed class CosmosDbLimitationFilterConverterFactory<TStaticSource> : IConverterFactory
        {
            public sealed class CosmosDbLimitationFilterConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            {
                private readonly Action<TStaticSource> _filter;

                public CosmosDbLimitationFilterConverter(Action<TStaticSource> filter)
                {
                    _filter = filter;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TStaticSource staticSource)
                        _filter(staticSource);

                    value = default;
                    return false;
                }
            }

            private readonly Action<TStaticSource> _filter;

            public CosmosDbLimitationFilterConverterFactory(Action<TStaticSource> filter)
            {
                _filter = filter;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>() => typeof(TStaticSource).IsAssignableFrom(typeof(TSource)) || typeof(TSource).IsAssignableFrom(typeof(TStaticSource))
                ? new CosmosDbLimitationFilterConverter<TSource, TTarget>(_filter)
                : null;
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
                    .StoreByteArraysAsBase64String()
                    .ConfigureSerializer(serializer => serializer
                        .Add(Create<CosmosDbKey, object>((key, env, recurse) => recurse.TransformTo<object>().From(
                            key.PartitionKey != null
                                ? new[] { key.PartitionKey, key.Id }
                                : (object)key.Id,
                            env)))
                        .Add(Create<FilterStep.ByTraversalStep, object>(static (step, env, recurse) => recurse.TransformTo<object>().From(
                            new WhereTraversalStep(
                                step.Traversal.Count > 0 && step.Traversal[0] is AsStep
                                    ? new MapStep(step.Traversal)
                                    : step.Traversal),
                            env)))
                        .Add(Create<HasKeyStep, object>((step, env, recurse) => step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase))
                            ? recurse.TransformTo<object>().From(
                                new WhereTraversalStep(Traversal.Empty.Push(
                                    KeyStep.Instance,
                                    new IsStep(p))),
                                env)
                            : default))
                        .Add(Create<NoneStep, object>((step, env, recurse) => recurse.TransformTo<object>().From(NoneWorkaround, env)))
                        .Add(Create<SkipStep, object>((step, env, recurse) => recurse.TransformTo<object>().From(new RangeStep(step.Count, -1, step.Scope), env)))
                        .Add(new CosmosDbLimitationFilterConverterFactory<LimitStep>(step =>
                        {
                            if (step.Count > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");
                        }))
                        .Add(new CosmosDbLimitationFilterConverterFactory<TailStep>(step =>
                        {
                            if (step.Count > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");
                        }))
                        .Add(new CosmosDbLimitationFilterConverterFactory<RangeStep>(step =>
                        {
                            if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");
                        }))
                        .Add(Create<Order, object>((order, env, recurse) => order.Equals(Order.Asc)
                            ? recurse.TransformTo<object>().From(WorkaroundOrder.Incr, env)
                            : order.Equals(Order.Desc)
                                ? recurse.TransformTo<object>().From(WorkaroundOrder.Decr, env)
                                : default))
                        .ToGroovy())
                    .StoreTimeSpansAsNumbers());
        }
    }
}
