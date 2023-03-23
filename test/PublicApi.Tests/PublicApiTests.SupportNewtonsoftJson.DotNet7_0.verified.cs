namespace ExRam.Gremlinq.Core
{
    public static class ConfiguratorExtensions
    {
        public static TConfigurator UseNewtonsoftJson<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : ExRam.Gremlinq.Core.IGremlinqConfigurator<TConfigurator> { }
    }
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment RegisterNativeType<TNative, TSerialized>(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, System.Func<TNative, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer, TSerialized> serializer, System.Func<Newtonsoft.Json.Linq.JValue, ExRam.Gremlinq.Core.IGremlinQueryEnvironment, ExRam.Gremlinq.Core.Transformation.ITransformer, ExRam.Gremlinq.Core.Transformation.ITransformer, TNative> deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseNewtonsoftJson(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
    public static class GremlinQuerySourceExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQuerySource UseNewtonsoftJson(this ExRam.Gremlinq.Core.IGremlinQuerySource source) { }
    }
}