using ExRam.Gremlinq.Providers.CosmosDb.AspNet;
using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static GremlinqSetup UseCosmosDb(this GremlinqSetup setup)
        {
            return new GremlinqSetup(setup.ServiceCollection.AddSingleton<IGremlinQueryEnvironmentTransformation, UseCosmosDbGremlinQueryEnvironmentTransformation>());
        }
    }
}
