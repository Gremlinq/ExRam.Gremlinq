using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.AspNet
{
    public static class GremlinqServicesBuilderExtensions
    {
        public static IGremlinqServicesBuilder UseNewtonsoftJson(this IGremlinqServicesBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .ConfigureQuerySource((source, _) => source
                    .ConfigureEnvironment(env => env
                        .UseNewtonsoftJson()));
        }
    }
}
