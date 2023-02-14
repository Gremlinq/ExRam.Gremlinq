namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializerExtensions
    {
        public static ExRam.Gremlinq.Core.Deserialization.IGremlinQueryFragmentDeserializer AddNewtonsoftJson(this ExRam.Gremlinq.Core.Deserialization.IGremlinQueryFragmentDeserializer deserializer) { }
    }
}
namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static Newtonsoft.Json.JsonSerializer GetJsonSerializer(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment, ExRam.Gremlinq.Core.Deserialization.IGremlinQueryFragmentDeserializer deserializer) { }
        public static ExRam.Gremlinq.Core.IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this ExRam.Gremlinq.Core.IGremlinQueryEnvironment environment) { }
    }
}