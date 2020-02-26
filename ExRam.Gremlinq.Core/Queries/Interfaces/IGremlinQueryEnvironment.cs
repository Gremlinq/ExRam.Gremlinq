using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IGremlinQueryEnvironment ConfigureOptions(Func<IImmutableDictionary<GremlinqOption, object>, IImmutableDictionary<GremlinqOption, object>> optionsTransformation);
        IGremlinQueryEnvironment ConfigureFeatureSet(Func<FeatureSet, FeatureSet> featureSetTransformation);

        IGremlinQueryEnvironment ConfigureSerializer(Func<IGremlinQuerySerializer, IGremlinQuerySerializer> serializerTransformation);
        IGremlinQueryEnvironment ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer, IGremlinQueryExecutionResultDeserializer> deserializerTransformation);
        IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);

        ILogger Logger { get; }
        IGraphModel Model { get; }
        FeatureSet FeatureSet { get; }
        IGremlinQueryExecutor Executor { get; }
        IGremlinQuerySerializer Serializer { get; }
        IImmutableDictionary<GremlinqOption, object> Options { get; }
        IGremlinQueryExecutionResultDeserializer Deserializer { get; }
    }
}
