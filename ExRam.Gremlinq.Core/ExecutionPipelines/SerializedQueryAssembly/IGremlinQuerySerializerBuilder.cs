using System;

namespace ExRam.Gremlinq.Core
{
    public delegate void AtomSerializer<TAtom>(TAtom atom, ISerializedGremlinQueryAssembler serializer, Action<TAtom> baseSerializer, Action<object> recurse);

    public interface IGremlinQuerySerializerBuilder
    {
        IGremlinQuerySerializerBuilder OverrideAtom<TAtom>(AtomSerializer<TAtom> atomSerializer);

        IGremlinQuerySerializerBuilder ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation);

        IGremlinQuerySerializer Build();
    }
}
