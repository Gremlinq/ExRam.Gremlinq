namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class DeserializerExtensions
    {
        public static ExRam.Gremlinq.Core.Transformation.ITransformer AddNewtonsoftJson(this ExRam.Gremlinq.Core.Transformation.ITransformer deserializer) { }
    }
}
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseGraphSon2(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment UseGraphSon3(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
}