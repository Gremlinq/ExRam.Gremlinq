using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource
    {
        IConfigurableGremlinQuerySource WithLogger(ILogger logger);
        IConfigurableGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource WithModel(IGraphModel model);
        IConfigurableGremlinQuerySource WithExecutor(IGremlinQueryExecutor executor);
    }
}
