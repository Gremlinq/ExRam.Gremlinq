namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseGremlinServer(this ExRam.Gremlinq.Core.IConfigurableGremlinQuerySource source, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketConfigurator, ExRam.Gremlinq.Core.IGremlinQuerySourceTransformation> builderAction) { }
    }
}
namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class GremlinServerGremlinqOptions
    {
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<bool> WorkaroundTinkerpop2112;
    }
}