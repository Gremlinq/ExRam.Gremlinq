using System;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public static class NeptuneConfiguratorExtensions
    {
        public static INeptuneConfigurator AtLocalhost(this INeptuneConfigurator configurator)
        {
            return configurator.At(new Uri("ws://localhost:8182"));
        }
    }
}
