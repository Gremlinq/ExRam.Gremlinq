using System;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);
        IGremlinQueryEnvironment ConfigureOptions(Func<GremlinqOptions, GremlinqOptions> optionsTransformation);
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IGremlinQueryEnvironment ConfigureExecutionPipeline(Func<IGremlinQueryExecutionPipeline, IGremlinQueryExecutionPipeline> pipelineTransformation);

        ILogger Logger { get; }
        GremlinqOptions Options { get; }
        IGraphModel Model { get; }
        IGremlinQueryExecutionPipeline Pipeline { get; }
    }
}
