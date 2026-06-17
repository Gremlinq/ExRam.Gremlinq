using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Provides Neptune-specific extension methods for <see cref="IGremlinQuerySource"/>.
    /// </summary>
    public static class GremlinQuerySourceExtensions
    {
        private sealed class NeptuneConfigurator : INeptuneConfigurator
        {
            public static readonly NeptuneConfigurator Default = new(WebSocketGremlinqClientFactory.LocalHost.Pool(), _ => _);

            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _querySourceTransformation;
            private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> _clientFactory;

            private NeptuneConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory, Func<IGremlinQuerySource, IGremlinQuerySource> querySourceTransformation)
            {
                _clientFactory = clientFactory;
                _querySourceTransformation = querySourceTransformation;
            }

            public INeptuneConfigurator ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new NeptuneConfigurator(
                transformation(_clientFactory),
                _querySourceTransformation);

            public INeptuneConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new NeptuneConfigurator(
                _clientFactory,
                _ => transformation(_querySourceTransformation(_)));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _querySourceTransformation
                .Invoke(source
                    .ConfigureEnvironment(environment => environment
                        .UseExecutor(_clientFactory
                            .Log()
                            .ToExecutor())));
        }

        private static readonly StepLabel<bool> UseDFEStepLabel = "Neptune#useDFE";

        /// <summary>
        /// Enables or disables the Neptune DFE (Deep Feature Engine) query engine.
        /// </summary>
        /// <param name="source">The query source to configure.</param>
        /// <param name="enabled">Whether to enable DFE.</param>
        public static IGremlinQuerySource UseDFE(this IGremlinQuerySource source, bool enabled = true)
        {
            ArgumentNullException.ThrowIfNull(source);

            return source
                .WithSideEffect(UseDFEStepLabel, enabled);
        }

        /// <summary>
        /// Configures the query source to use the AWS Neptune provider.
        /// </summary>
        /// <typeparam name="TVertexBase">The base type for all vertex entities.</typeparam>
        /// <typeparam name="TEdgeBase">The base type for all edge entities.</typeparam>
        /// <param name="source">The query source to configure.</param>
        /// <param name="configuratorTransformation">A function that configures the Neptune provider.</param>
        public static IGremlinQuerySource UseNeptune<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(configuratorTransformation);

            return configuratorTransformation
                .Invoke(NeptuneConfigurator.Default)
                .Transform(source
                .ConfigureEnvironment(environment => environment
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertexBase, TEdgeBase>())
                    .ConfigureFeatureSet(featureSet => featureSet
                        .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                        .ConfigureVariableFeatures(_ => VariableFeatures.None)
                        .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                        .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                        .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                        .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.WorkaroundRangeInconsistencies, true))
                    .ConfigureNativeTypes(nativeTypes => nativeTypes
                        .Remove(typeof(byte[]))
                        .Remove(typeof(TimeSpan)))
                    .AddGraphSonBinarySupport()
                    .ConfigureSerializer(serializer => serializer
                        .Add(ConverterFactory
                            .Create<PropertyStep.ByKeyStep, Instruction>((step, env, _, recurse) => Cardinality.List.Equals(step.Cardinality)
                                ? recurse
                                    .TransformTo<Instruction>()
                                    .From(new PropertyStep.ByKeyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set), env)
                                : null)))
                    .ConfigureDeserializer(deserializer => deserializer
                        .AsIncomplete())))
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(executor => executor
                    .TransformExecutionException(ex => ex.TryGetNeptuneGremlinQueryExecutionException() ?? ex)));
        }
    }
}
