using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IWebSocketGremlinQueryEnvironmentBuilderTransformation
    {
        IWebSocketGremlinQueryExecutorBuilder Transform(IWebSocketGremlinQueryExecutorBuilder builder);
    }
}
