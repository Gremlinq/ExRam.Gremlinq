using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.Configuration;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup)
        {
            return setup 
                .UseProvider<ICosmosDbConfigurator>(
                    "CosmosDb",
                    (e, f) => e.UseCosmosDb(f),
                    (configurator, configuration) => configurator
                        .At(
                            configuration.GetRequiredConfiguration("Uri"),
                            configuration.GetRequiredConfiguration("Database"),
                            configuration.GetRequiredConfiguration("Graph"))
                        .AuthenticateBy(configuration.GetRequiredConfiguration("AuthKey")));
        }

        public static GremlinqSetup UseCosmosDb<TVertex, TEdge>(this GremlinqSetup setup, Expression<Func<TVertex, object>> partitionKeyExpression)
        {
            return setup
                .UseCosmosDb()
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())
                        .ConfigureProperties(model => model
                            .ConfigureElement<TVertex>(conf => conf
                                .IgnoreOnUpdate(partitionKeyExpression)))));
        }
    }
}
