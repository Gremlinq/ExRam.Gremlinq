using Microsoft.Extensions.DependencyInjection;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public readonly struct ProviderSetup<TConfigurator>
         where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        public ProviderSetup(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }
    }
}
