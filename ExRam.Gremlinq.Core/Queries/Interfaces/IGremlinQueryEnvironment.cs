using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation);
        IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IGremlinQueryEnvironment ConfigureExecutionPipeline(Func<IGremlinQueryExecutionPipeline, IGremlinQueryExecutionPipeline> pipelineTransformation);
        IGremlinQueryEnvironment ConfigureOptions(Func<IImmutableDictionary<GremlinqOption, object>, IImmutableDictionary<GremlinqOption, object>> optionsTransformation);

        ILogger Logger { get; }
        IGraphModel Model { get; }
        IGremlinQueryExecutionPipeline Pipeline { get; }
        IImmutableDictionary<GremlinqOption, object> Options { get; }
    }
}
