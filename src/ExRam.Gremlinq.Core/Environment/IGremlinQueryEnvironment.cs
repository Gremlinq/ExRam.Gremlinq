using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation);
        IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation);
        IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation);
        IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation);
        IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation);
        IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);


        ILogger Logger { get; }
        IGraphModel Model { get; }
        IFeatureSet FeatureSet { get; }
        ITransformer Serializer { get; }
        IGremlinqOptions Options { get; }
        ITransformer Deserializer { get; }
        IGremlinQueryDebugger Debugger { get; }
        IGremlinQueryExecutor Executor { get; }
    }
}
