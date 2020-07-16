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
                    .UseJanusGraph(builder => builder.Configure(_configuration, _webSocketTransformations));
            }
        }

        public static GremlinqSetup UseJanusGraph(this GremlinqSetup setup)
        {
            return setup
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseJanusGraphGremlinQueryEnvironmentTransformation>());
        }
    }
}
