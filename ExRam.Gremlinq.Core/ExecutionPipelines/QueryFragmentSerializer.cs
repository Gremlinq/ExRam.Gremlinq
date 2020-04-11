using System;

namespace ExRam.Gremlinq.Core
{
    public delegate object QueryFragmentSerializer<TFragment>(TFragment fragment, Func<TFragment, object> baseSerializer, Func<object, object> recurse);
}
