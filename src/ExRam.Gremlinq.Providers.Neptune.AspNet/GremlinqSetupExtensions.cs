using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseNeptuneGremlinQueryEnvironmentTransformation : IGremlinQueryEnvironmentTransformation
        {
            private readonly IConfiguration _configuration;
            private readonly IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> _webSocketTransformations;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseNeptuneGremlinQueryEnvironmentTransformation(
                IGremlinqConfiguration configuration,
                IEnumerable<IWebSocketGremlinQueryExecutorBuilderTransformation> webSocketTransformations)
            {
                _webSocketTransformations = webSocketTransformations;
                _configuration = configuration
                    .GetSection("Neptune");
            }

            public IGremlinQueryEnvironment Transform(IGremlinQueryEnvironment environment)
            {
                return environment
                    .UseNeptune(builder => builder.Configure(_configuration)
                        .Transform(_webSocketTransformations));
            }
        }

        public static GremlinqSetup UseNeptune(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseNeptuneGremlinQueryEnvironmentTransformation>());
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
