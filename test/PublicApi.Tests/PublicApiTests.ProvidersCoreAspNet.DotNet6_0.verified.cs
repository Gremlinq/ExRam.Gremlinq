namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> ConfigureWebSocket<TConfigurator>(this ExRam.Gremlinq.Core.AspNet.IGremlinqServicesBuilder<TConfigurator> builder)
            where TConfigurator : ExRam.Gremlinq.Providers.Core.IWebSocketProviderConfigurator<TConfigurator> { }
    }
}