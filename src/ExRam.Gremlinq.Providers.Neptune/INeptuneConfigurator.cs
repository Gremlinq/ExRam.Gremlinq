using System;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurator : IProviderConfigurator<INeptuneConfigurator>
    {
        INeptuneConfigurator At(Uri uri);
    }
}
