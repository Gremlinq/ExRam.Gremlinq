using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseJanusGraphGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;
            private readonly IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> _webSocketTransformations;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseJanusGraphGremlinQueryEnvironmentTransformation(
                IGremlinqConfiguration configuration,
                IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> webSocketTransformations)
            {
                _webSocketTransformations = webSocketTransformations;
                _configuration = configuration
                    .GetSection("JanusGraph");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseJanusGraph(builder => builder
                        .At(new Uri("ws://localhost:8182"))
                        .ConfigureWebSocket(_ => _
                            .Configure(_configuration)
                            .Transform(_webSocketTransformations)));
            }
        }

        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseJanusGraphGremlinQueryEnvironmentTransformation>());
        }

        public static GremlinqSetup UseJanusGraph<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseJanusGraph()
                .UseModel(GraphModel
                    .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()));
        }
    }
}
