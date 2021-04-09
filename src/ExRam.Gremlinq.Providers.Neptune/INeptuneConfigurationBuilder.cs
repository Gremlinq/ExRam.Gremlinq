using System;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public interface INeptuneConfigurationBuilder
    {
        INeptuneConfigurationBuilderWithUri At(Uri uri);
    }
}
