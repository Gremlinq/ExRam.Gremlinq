using System;

namespace ExRam.Gremlinq.Core
{
    public delegate void AtomSerializer<TAtom>(TAtom atom, ISerializedGremlinQueryAssembler assembler, Action<TAtom> baseSerializer, Action<object> recurse);
}