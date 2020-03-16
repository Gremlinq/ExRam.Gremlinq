using ExRam.Gremlinq.Providers.CosmosDb.AspNet;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqOptionsExtensions
    {
        public static GremlinqOptions UseCosmosDb(this GremlinqOptions options)
        {
            return new GremlinqOptions(options.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseCosmosDbGremlinQueryEnvironmentTransformation>());
        }
    }
}
