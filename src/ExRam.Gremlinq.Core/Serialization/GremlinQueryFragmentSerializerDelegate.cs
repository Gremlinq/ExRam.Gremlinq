using System;

namespace ExRam.Gremlinq.Core
{
    public delegate object? GremlinQueryFragmentSerializerDelegate<TFragment>(TFragment fragment, IGremlinQueryEnvironment environment, Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object?> overridden, IGremlinQueryFragmentSerializer recurse);
}
