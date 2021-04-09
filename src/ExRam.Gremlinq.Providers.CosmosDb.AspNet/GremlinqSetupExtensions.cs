using System;
using System.Linq.Expressions;
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
            public UseCosmosDbGremlinQueryEnvironmentTransformation(
                IGremlinqConfiguration configuration)
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
                            .ConfigureWebSocket(builder => builder
                                .Configure(_configuration));
                    });
            }
        }

        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseCosmosDbGremlinQueryEnvironmentTransformation>());
        }

        public static GremlinqSetup UseCosmosDb<TVertex, TEdge>(this GremlinqSetup setup, Expression<Func<TVertex, object>> partitionKeyExpression)
        {
            return setup
                .UseCosmosDb()
                .UseModel(GraphModel
                    .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())
                    .ConfigureProperties(model => model
                        .ConfigureElement<TVertex>(conf => conf
                            .IgnoreOnUpdate(partitionKeyExpression))));
        }
    }
}
