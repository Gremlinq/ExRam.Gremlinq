using System;

namespace ExRam.Gremlinq.Core
{
    public delegate object AtomSerializer<TAtom>(TAtom atom, Func<TAtom, object> baseSerializer, Func<object, object> recurse);
}
