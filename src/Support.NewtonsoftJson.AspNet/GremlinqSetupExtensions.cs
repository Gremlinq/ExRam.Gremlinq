namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static IGremlinqServicesBuilder UseNewtonsoftJson(this IGremlinqServicesBuilder setup) => setup
            .ConfigureQuerySource(source => source
                .UseNewtonsoftJson());
    }
}
