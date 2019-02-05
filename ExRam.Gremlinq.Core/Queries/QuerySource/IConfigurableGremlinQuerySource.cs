using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource
    {
        IConfigurableGremlinQuerySource WithName(string name);
        IConfigurableGremlinQuerySource WithLogger(ILogger logger);
        IConfigurableGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource WithoutStrategies(params string[] strategies);
        IConfigurableGremlinQuerySource WithModel(IGraphModel model);
        IConfigurableGremlinQuerySource WithExecutor(IGremlinQueryExecutor executor);
    }
}
