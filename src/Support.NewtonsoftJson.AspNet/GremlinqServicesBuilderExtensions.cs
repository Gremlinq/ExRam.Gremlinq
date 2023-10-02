namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder UseNewtonsoftJson(this IGremlinqServicesBuilder builder) => builder
            .ConfigureQuerySource((source, section) => source
                .UseNewtonsoftJson());
    }
}
