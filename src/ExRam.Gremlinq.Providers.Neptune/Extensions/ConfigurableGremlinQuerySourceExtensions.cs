using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core.Transformation;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class NeptuneConfigurator : INeptuneConfigurator
        {
            private readonly WebSocketProviderConfigurator _baseConfigurator;

            public NeptuneConfigurator() : this(new WebSocketProviderConfigurator())
            {
            }

            public NeptuneConfigurator(WebSocketProviderConfigurator baseConfigurator)
            {
                _baseConfigurator = baseConfigurator;
            }

            public INeptuneConfigurator ConfigureAlias(Func<string, string> transformation) => new NeptuneConfigurator(_baseConfigurator.ConfigureAlias(transformation));

            public INeptuneConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new NeptuneConfigurator(_baseConfigurator.ConfigureClientFactory(transformation));

            public INeptuneConfigurator ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new NeptuneConfigurator(_baseConfigurator.ConfigureServer(transformation));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => _baseConfigurator.Transform(source);
        }

        public static IGremlinQuerySource UseNeptune(this IConfigurableGremlinQuerySource source, Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(new NeptuneConfigurator())
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                            .ConfigureVariableFeatures(_ => VariableFeatures.None)
                            .ConfigureVertexFeatures(_ => VertexFeatures.AddVertices | VertexFeatures.RemoveVertices | VertexFeatures.MultiProperties | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty | VertexFeatures.StringIds)
                            .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.NumericIds | VertexPropertyFeatures.StringIds | VertexPropertyFeatures.Properties | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                            .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty | EdgeFeatures.NumericIds | EdgeFeatures.StringIds | EdgeFeatures.UuidIds | EdgeFeatures.CustomIds | EdgeFeatures.AnyIds)
                            .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                        .UseGraphSon3()
                        .UseNewtonsoftJson()))
                .ConfigureEnvironment(environment => environment
                    .ConfigureSerializer(serializer => serializer
                        .Add(ConverterFactory
                            .Create<PropertyStep.ByKeyStep, PropertyStep.ByKeyStep>((step, env, recurse) => Cardinality.List.Equals(step.Cardinality)
                                ? new PropertyStep.ByKeyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set)
                                : default)
                            .AutoRecurse<PropertyStep.ByKeyStep>()))
                    .StoreTimeSpansAsNumbers()
                    .StoreByteArraysAsBase64String());
        }
    }
}
