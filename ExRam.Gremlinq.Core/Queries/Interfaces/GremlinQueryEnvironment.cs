using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironment
    {
        private sealed class DefaultGremlinQueryEnvironmentImpl : IGremlinQueryEnvironment
        {
            public ILogger Logger { get; } = NullLogger.Instance;
            public Options Options { get; } = default;
            public IGraphModel Model { get; } = GraphModel.Empty;
            public IGremlinQueryExecutor Executor { get; } = GremlinQueryExecutor.Invalid;
        }

        public static readonly IGremlinQueryEnvironment Default = new DefaultGremlinQueryEnvironmentImpl();
    }
}