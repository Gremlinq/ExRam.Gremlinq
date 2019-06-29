using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironment
    {
        private sealed class DefaultGremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public GremlinqOptions Options { get; } = default;
            public IGraphModel Model { get; } = GraphModel.Empty;
            public ILogger Logger { get; } = NullLogger.Instance;
            public IGremlinQueryExecutionPipeline Pipeline { get; } = GremlinQueryExecutionPipeline.Invalid;
        }

        public static readonly IGremlinQueryEnvironment Default = new DefaultGremlinQueryEnvironmentImpl();
    }
}
