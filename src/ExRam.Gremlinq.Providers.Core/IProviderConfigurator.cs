using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfigurator<out TConfigurator> : IGremlinqConfigurator<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {

    }
}
