namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static IGremlinqSetup UseNewtonsoftJson(this IGremlinqSetup setup) => setup
            .ConfigureQuerySource(source => source
                .UseNewtonsoftJson());
    }
}
