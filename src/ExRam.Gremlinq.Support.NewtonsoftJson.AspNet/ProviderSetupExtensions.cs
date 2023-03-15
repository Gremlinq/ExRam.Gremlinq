using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        public static ProviderSetup<TConfigurator> UseNewtonsoftJson<TConfigurator>(this ProviderSetup<TConfigurator> setup)
            where TConfigurator : IProviderConfigurator<TConfigurator> => setup
                .Configure((configurator, _) => configurator
                    .UseNewtonsoftJson());
    }
}
