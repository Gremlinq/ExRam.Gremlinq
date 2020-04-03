using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseNeptuneGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseNeptuneGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("Neptune");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseNeptune(builder => builder.Configure(_configuration));
            }
        }

        public static GremlinqSetup UseNeptune(this GremlinqSetup setup)
        {
            return new GremlinqSetup(setup.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseNeptuneGremlinQueryEnvironmentTransformation>());
        }
    }
}
