using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class ServiceCollectionExtensions
    {
        private sealed class UseGremlinServerGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;

            public UseGremlinServerGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("GremlinServer");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseGremlinServer(builder => builder.Configure(_configuration));
            }
        }

        public static GremlinqOptions UseGremlinServer(this GremlinqOptions options)
        {
            return new GremlinqOptions(options.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseGremlinServerGremlinQueryEnvironmentTransformation>());
        }
    }
}
