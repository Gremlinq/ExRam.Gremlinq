namespace ExRam.Gremlinq.Core
{
    public interface IConfigurableGremlinQuerySource : IGremlinQuerySource
    { 
        IConfigurableGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IConfigurableGremlinQuerySource WithModel(IGraphModel model);
        IConfigurableGremlinQuerySource WithExecutor(IGremlinQueryExecutor executor);
    }
}
