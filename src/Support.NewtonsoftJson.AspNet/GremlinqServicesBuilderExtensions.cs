using ExRam.Gremlinq.Core.AspNet;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.AspNet
{
    /// <summary>Extension methods for configuring Newtonsoft.Json on <see cref="IGremlinqServicesBuilder"/>.</summary>
    public static class GremlinqServicesBuilderExtensions
    {
        /// <summary>Configures the services builder to use Newtonsoft.Json for serialization.</summary>
        /// <param name="builder">The services builder.</param>
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
