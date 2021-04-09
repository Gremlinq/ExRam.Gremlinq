using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseJanusGraphGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseJanusGraphGremlinQuerySourceTransformation(
                IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("JanusGraph");
            }

            public IConfigurableGremlinQuerySource Transform(IConfigurableGremlinQuerySource source)
            {
                return source
                   .UseJanusGraph(builder => builder
                        .At(new Uri("ws://localhost:8182"))
                        .ConfigureWebSocket(_ => _
                            .Configure(_configuration)));
            }
        }

        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQuerySourceTransformation, UseJanusGraphGremlinQuerySourceTransformation>());
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
