using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializer
    {
        IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer);

        IGremlinQuerySerializer ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation);

        object Serialize(IGremlinQuery query);
    }
}
