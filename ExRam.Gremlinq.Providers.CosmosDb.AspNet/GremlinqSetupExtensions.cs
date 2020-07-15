using Microsoft.Extensions.DependencyInjection;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseCosmosDbGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseCosmosDbGremlinQueryEnvironmentTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("CosmosDb");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseCosmosDb(builder =>
                    {
                        return builder
                            .At(
                                _configuration.GetRequiredConfiguration("Uri"),
                                _configuration.GetRequiredConfiguration("Database"),
                                _configuration.GetRequiredConfiguration("Graph"))
                            .AuthenticateBy(_configuration.GetRequiredConfiguration("AuthKey"))
                            .ConfigureWebSocket(webSocketBuilder => webSocketBuilder
                                .Configure(_configuration));
                    });
            }
        }

        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup)
        {
            return new GremlinqSetup(setup.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseCosmosDbGremlinQueryEnvironmentTransformation>());
        }
    }
}
