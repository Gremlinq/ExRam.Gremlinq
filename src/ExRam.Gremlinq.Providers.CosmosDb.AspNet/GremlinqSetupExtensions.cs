using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class CosmosDbConfiguratorTransformation : IProviderConfiguratorTransformation<ICosmosDbConfigurator>
        {
            private readonly IProviderConfiguration _configuration;

            public CosmosDbConfiguratorTransformation(IProviderConfiguration configuration)
            {
                _configuration = configuration;
            }

            public ICosmosDbConfigurator Transform(ICosmosDbConfigurator configurator)
            {
                if (_configuration["Database"] is { } databaseName)
                    configurator = configurator.OnDatabase(databaseName);

                if (_configuration["Graph"] is { } graphName)
                    configurator = configurator.OnGraph(graphName);

                if (_configuration["AuthKey"] is { } authKey)
                    configurator = configurator.AuthenticateBy(authKey);

                return configurator;
            }
        }

        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup, Action<ProviderSetup<ICosmosDbConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseProvider(
                    "CosmosDb",
                    (source, configuratorTransformation) => source.UseCosmosDb(configuratorTransformation),
                    extraSetupAction)
                .RegisterTypes(serviceCollection => serviceCollection
                    .AddSingleton<IProviderConfiguratorTransformation<ICosmosDbConfigurator>, CosmosDbConfiguratorTransformation>());
        }

        public static GremlinqSetup UseCosmosDb<TVertex, TEdge>(this GremlinqSetup setup, Expression<Func<TVertex, object>> partitionKeyExpression, Action<ProviderSetup<ICosmosDbConfigurator>>? extraSetupAction = null)
        {
            return setup
                .UseCosmosDb(extraSetupAction)
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
