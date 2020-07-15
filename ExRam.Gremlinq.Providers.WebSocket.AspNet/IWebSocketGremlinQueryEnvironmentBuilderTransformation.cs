using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public interface IWebSocketGremlinQueryEnvironmentBuilderTransformation
    {
        IWebSocketGremlinQueryEnvironmentBuilder Transform(IWebSocketGremlinQueryEnvironmentBuilder builder);
    }
}
