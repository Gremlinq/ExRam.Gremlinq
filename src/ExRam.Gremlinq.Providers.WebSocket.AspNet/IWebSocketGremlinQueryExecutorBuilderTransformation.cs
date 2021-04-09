using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IWebSocketGremlinQueryExecutorBuilderTransformation
    {
        IWebSocketConfigurator Transform(IWebSocketConfigurator builder);
    }
}
