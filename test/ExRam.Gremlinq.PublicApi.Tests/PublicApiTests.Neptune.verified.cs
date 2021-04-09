namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseNeptune(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<ExRam.Gremlinq.Core.INeptuneConfigurationBuilder, ExRam.Gremlinq.Core.IGremlinQueryExecutorBuilder> transformation) { }
    }
    public interface INeptuneConfigurationBuilder
    {
        ExRam.Gremlinq.Core.INeptuneConfigurationBuilderWithUri At(System.Uri uri);
    }
    public interface INeptuneConfigurationBuilderWithUri : ExRam.Gremlinq.Core.IGremlinQueryExecutorBuilder
    {
        ExRam.Gremlinq.Core.IGremlinQueryExecutorBuilder ConfigureWebSocket(System.Func<ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder, ExRam.Gremlinq.Providers.WebSocket.IWebSocketGremlinQueryExecutorBuilder> transformation);
    }
}