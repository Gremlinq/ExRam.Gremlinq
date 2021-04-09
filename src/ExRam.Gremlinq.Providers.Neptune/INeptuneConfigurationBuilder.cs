using System;

namespace ExRam.Gremlinq.Core
{
    public interface INeptuneConfigurationBuilder
    {
        INeptuneConfigurationBuilderWithUri At(Uri uri);
    }
}
