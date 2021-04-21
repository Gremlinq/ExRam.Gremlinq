using System;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfigurator<out TConfigurator> : IGremlinQuerySourceTransformation
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        TConfigurator At(Uri uri);
    }
}
