using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.Core.AspNet;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<TConfigurator> UseNewtonsoftJson<TConfigurator>(this ProviderSetup<TConfigurator> setup)
            where TConfigurator : IProviderConfigurator<TConfigurator>  => setup
                .ConfigureQuerySource(source => source
                    .UseNewtonsoftJson());
    }
}
