using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseGremlinServerGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
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

        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup)
        {
            return new GremlinqSetup(setup.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseGremlinServerGremlinQueryEnvironmentTransformation>());
        }
    }
}
