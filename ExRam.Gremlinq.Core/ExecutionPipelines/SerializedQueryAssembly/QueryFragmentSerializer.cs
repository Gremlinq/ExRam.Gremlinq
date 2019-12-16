using System;

namespace ExRam.Gremlinq.Core
{
    public delegate object? QueryFragmentSerializer<TFragment>(TFragment atom, Func<TFragment, object?> baseSerializer, Func<object, object?> recurse);
}
