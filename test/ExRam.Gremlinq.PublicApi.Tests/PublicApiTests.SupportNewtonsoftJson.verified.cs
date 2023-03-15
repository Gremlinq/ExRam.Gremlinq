namespace ExRam.Gremlinq.Core
{
    public static class ConfiguratorExtensions
    {
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator> { }
    }
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseNewtonsoftJson(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
}