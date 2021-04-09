using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseNeptuneGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseNeptuneGremlinQuerySourceTransformation(
                IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("Neptune");
            }

            public IConfigurableGremlinQuerySource Transform(IConfigurableGremlinQuerySource source)
            {
                return source
                    .UseNeptune(builder => builder
                        .At(new Uri("ws://localhost:8182"))
                        .ConfigureWebSocket(_ => _
                            .Configure(_configuration)));
            }
        }

        public static GremlinqSetup UseNeptune(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQuerySourceTransformation, UseNeptuneGremlinQuerySourceTransformation>());
        }

        public static GremlinqSetup UseNeptune<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseNeptune()
                .UseModel(GraphModel
                    .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()));
        }
    }
}
