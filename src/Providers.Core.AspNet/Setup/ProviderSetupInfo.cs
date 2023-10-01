using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Core.AspNet
{
    internal sealed class ProviderSetupInfo<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        public ProviderSetupInfo(Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> providerChoice)
        {
            ProviderChoice = providerChoice;
        }

        public Func<IConfigurableGremlinQuerySource, Func<TConfigurator, IGremlinQuerySourceTransformation>, IGremlinQuerySource> ProviderChoice { get; }
    }
}
