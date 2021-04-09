using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        private sealed class UseGremlinServerGremlinQuerySourceTransformation : IGremlinQuerySourceTransformation
        {
            private readonly IConfiguration _configuration;

            // ReSharper disable once SuggestBaseTypeForParameter
            public UseGremlinServerGremlinQuerySourceTransformation(
                IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("GremlinServer");
            }

            public IConfigurableGremlinQuerySource Transform(IConfigurableGremlinQuerySource source)
            {
                return source
                    .UseGremlinServer(builder => builder
                        .Configure(_configuration));
            }
        }

        public static GremlinqSetup UseGremlinServer(this GremlinqSetup setup)
        {
            return setup
                .UseWebSocket()
                .RegisterTypes(serviceCollection => serviceCollection.AddSingleton<IGremlinQuerySourceTransformation, UseGremlinServerGremlinQuerySourceTransformation>());
        }

        public static GremlinqSetup UseGremlinServer<TVertex, TEdge>(this GremlinqSetup setup)
        {
            return setup
                .UseGremlinServer()
                .UseModel(GraphModel
                    .FromBaseTypes<TVertex, TEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()));
        }
    }
}
