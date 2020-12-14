using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IWebSocketGremlinQueryExecutorBuilderTransformation
    {
        IWebSocketGremlinQueryExecutorBuilder Transform(IWebSocketGremlinQueryExecutorBuilder builder);
    }
}
