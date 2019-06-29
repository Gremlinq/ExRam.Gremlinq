using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        ILogger Logger { get; }
        GremlinqOptions Options { get; }
        IGraphModel Model { get; }
        IGremlinQueryExecutionPipeline Pipeline { get; }
    }
}
