using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        ILogger Logger { get; }
        Options Options { get; }
        IGraphModel Model { get; }
        ServerCapabilities ServerCapabilities { get; }
        IGremlinQueryExecutionPipeline Pipeline { get; }
    }
}
