[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName="")]
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseGremlinServer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryEnvironmentBuilder> builderAction) { }
    }
}
namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class GremlinServerGremlinqOptions
    {
        public static readonly ExRam.Gremlinq.Core.GremlinqOption<bool> WorkaroundTinkerpop2112;
    }
}