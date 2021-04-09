using System;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator
    {
        INeptuneConfiguratorWithUri At(Uri uri);
    }
}
