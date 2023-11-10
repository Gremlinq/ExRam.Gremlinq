using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Driver.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class ConfigurableGremlinQuerySourceExtensions
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

        private record struct NeptuneErrorResponse(string? requestId, string? code, string? detailedMessage);

        public static IGremlinQuerySource UseNeptune<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<INeptuneConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            
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
                        .ConfigureNativeTypes(nativeTypes => nativeTypes
                            .Remove(typeof(byte[]))
                            .Remove(typeof(TimeSpan)))
                        .UseGraphSon3()
                        .ConfigureSerializer(serializer => serializer
                            .Add(ConverterFactory
                                .Create<PropertyStep.ByKeyStep, PropertyStep.ByKeyStep>((step, _, _, _) => Cardinality.List.Equals(step.Cardinality)
                                    ? new PropertyStep.ByKeyStep(step.Key, step.Value, step.MetaProperties, Cardinality.Set)
                                    : default)))))
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(executor => executor
                        .TransformExecutionException(ex =>
                        {
                            if (ex.InnerException is ResponseException responseException)
                            {
                                var statusCodeString = responseException.StatusCode.ToString();

                                if (responseException.Message.StartsWith(statusCodeString) && responseException.Message.Length > statusCodeString.Length)
                                {
                                    try
                                    {
                                        var response = JsonSerializer.Deserialize<NeptuneErrorResponse>(responseException.Message.AsSpan()[(statusCodeString.Length + 1)..], serializerOptions);

                                        if (response.code is { Length: > 0 } errorCode && NeptuneErrorCode.From(errorCode) is { } neptuneErrorCode)
                                        {
                                            return response.detailedMessage is { Length: > 0 } detailedMessage
                                                ? new NeptuneGremlinQueryExecutionException(neptuneErrorCode, ex.ExecutionContext, detailedMessage, ex)
                                                : new NeptuneGremlinQueryExecutionException(neptuneErrorCode, ex.ExecutionContext, ex);
                                        }
                                    }
                                    catch (JsonException)
                                    {

                                    }
                                }
                            }

                            return ex;
                        })));
        }
    }
}
