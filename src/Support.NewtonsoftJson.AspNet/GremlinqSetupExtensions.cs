namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseNewtonsoftJson(this GremlinqSetup setup) => setup
            .ConfigureQuerySource(source => source
                .UseNewtonsoftJson());
    }
}
