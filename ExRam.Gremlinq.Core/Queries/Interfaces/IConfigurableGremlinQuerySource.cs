using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource, IGremlinQueryEnvironment
    {
        IConfigurableGremlinQuerySource UseName(string name);
        IConfigurableGremlinQuerySource UseLogger(ILogger logger);
        IConfigurableGremlinQuerySource AddStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource RemoveStrategies(params string[] strategies);

        IConfigurableGremlinQuerySource ConfigureOptions(Func<IGremlinQueryEnvironment, Options, Options> optionsTransformation);
        IConfigurableGremlinQuerySource ConfigureModel(Func<IGremlinQueryEnvironment, IGraphModel, IGraphModel> modelTransformation);
        IConfigurableGremlinQuerySource ConfigureExecutionPipeline(Func<IGremlinQueryEnvironment, IGremlinQueryExecutionPipeline, IGremlinQueryExecutionPipeline> pipelineTransformation);

        string Name { get; }
        ImmutableList<string> ExcludedStrategyNames { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
