using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseCosmosDbGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseCosmosDbGremlinQuerySourceTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("CosmosDb");
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
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
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQuerySourceTransformation, UseCosmosDbGremlinQuerySourceTransformation>());
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
