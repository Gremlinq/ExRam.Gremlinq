namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseGremlinServer(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> configuratorTransformation) { }
    }
}
namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public interface IGremlinServerConfigurator : ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation, ExRam.Gremlinq.Providers.Core.IProviderConfigurator<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator>, ExRam.Gremlinq.Providers.WebSocket.IWebSocketProviderConfigurator<ExRam.Gremlinq.Providers.GremlinServer.IGremlinServerConfigurator> { }
}