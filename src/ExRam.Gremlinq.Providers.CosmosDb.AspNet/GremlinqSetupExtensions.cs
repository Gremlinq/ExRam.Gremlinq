using System.Collections.Generic;
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
            private readonly IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> _webSocketTransformations;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseCosmosDbGremlinQueryEnvironmentTransformation(
                IGremlinqConfiguration configuration,
                IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> webSocketTransformations)
            {
                _webSocketTransformations = webSocketTransformations;
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
                            .ConfigureWebSocket(builder => builder
                                .Configure(_configuration)
                                .Transform(_webSocketTransformations));
                    });
            }
        }

        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseCosmosDbGremlinQueryEnvironmentTransformation>());
        }
    }
}
