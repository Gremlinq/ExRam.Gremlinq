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

        IConfigurableGremlinQuerySource ConfigureOptions(Func<Options, Options> optionsTransformation);
        IConfigurableGremlinQuerySource ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation);
        IConfigurableGremlinQuerySource ConfigurePipeline(Func<IGremlinExecutionPipelineBuilderStage1, IGremlinQueryExecutionPipeline> builderTransformation);

        string Name { get; }
        ImmutableList<string> ExcludedStrategyNames { get; }
        ImmutableList<IGremlinQueryStrategy> IncludedStrategies { get; }
    }
}
