using System;
using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource, IGremlinQueryEnvironment
    {
        IConfigurableGremlinQuerySource WithName(string name);
        IConfigurableGremlinQuerySource WithLogger(ILogger logger);
        IConfigurableGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies);

        IConfigurableGremlinQuerySource ConfigureOptions(Func<IGremlinQueryEnvironment, Options, Options> optionsTransformation);
        IConfigurableGremlinQuerySource ConfigureModel(Func<IGremlinQueryEnvironment, IGraphModel, IGraphModel> modelTransformation);
        IConfigurableGremlinQuerySource ConfigureExecution(Func<IGremlinQueryEnvironment, IGremlinExecutionPipelineBuilderStage1, IGremlinQueryExecutionPipeline> builderTransformation);

        string Name { get; }
        ImmutableList<string> ExcludedStrategyNames { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
