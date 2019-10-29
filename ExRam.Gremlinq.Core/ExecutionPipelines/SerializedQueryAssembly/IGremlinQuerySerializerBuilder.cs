using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializerBuilder
    {
        IGremlinQuerySerializerBuilder OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer);

        IGremlinQuerySerializerBuilder ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation);

        IGremlinQuerySerializer Build();
    }
}
