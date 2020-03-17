using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqOptionsExtensions
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

        public static GremlinqOptions UseNeptune(this GremlinqOptions options)
        {
            return new GremlinqOptions(options.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseNeptuneGremlinQueryEnvironmentTransformation>());
        }
    }
}
