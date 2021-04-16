namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfiguratorTransformation
    {
        IWebSocketConfigurator Transform(IWebSocketConfigurator configurator);
    }
}
