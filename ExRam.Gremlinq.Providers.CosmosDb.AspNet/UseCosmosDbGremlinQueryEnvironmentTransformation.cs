using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Providers.CosmosDb.AspNet
{
    internal sealed class UseCosmosDbGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
    {
        private readonly IConfiguration _configuration;

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
                        .ConfigureWebSocket(webSocketBuilder => webSocketBuilder
                            .Configure(_configuration))
                        .At(
                            new Uri(_configuration.GetRequiredConfiguration("Uri")),
                            _configuration.GetRequiredConfiguration("Database"),
                            _configuration.GetRequiredConfiguration("Graph"))
                        .AuthenticateBy(_configuration.GetRequiredConfiguration("AuthKey"));
                });
        }
    }
}
