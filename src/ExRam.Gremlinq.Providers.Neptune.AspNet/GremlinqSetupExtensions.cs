using System;
using ExRam.Gremlinq.Providers.Neptune;
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
            public UseNeptuneGremlinQuerySourceTransformation(IGremlinqConfiguration configuration)
            {
                _configuration = configuration
                    .GetSection("Neptune");
            }

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
                    .UseNeptune(configurator =>
                    {
                        configurator = configurator
                            .At(new Uri("ws://localhost:8182"));

                        var transformation = (_configuration["ElasticSearchEndPoint"] is { } endPoint)
                            ? configurator.UseElasticSearch(new Uri(endPoint))
                            : configurator;

                        return transformation
                            .ConfigureWebSocket(_ => _
                                .Configure(_configuration));
                    });
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
