using ExRam.Gremlinq.Core.Serialization;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryEnvironment
    {
        ILogger Logger { get; }
        Options Options { get; }
        IGraphModel Model { get; }
        IGremlinQueryExecutor Executor { get; }
        IGremlinQueryElementVisitorCollection Visitors { get; }
    }
}
