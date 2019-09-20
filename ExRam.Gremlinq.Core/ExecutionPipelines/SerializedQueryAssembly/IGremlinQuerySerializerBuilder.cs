using System;

namespace ExRam.Gremlinq.Core
{
    public delegate void AtomSerializationHandler<TAtom>(TAtom atom, ISerializedGremlinQueryAssembler serializer, Action<TAtom> baseSerializer, Action<object> recurse);

    public interface IGremlinQuerySerializerBuilder
    {
        IGremlinQuerySerializerBuilder OverrideAtomSerializationHandler<TAtom>(AtomSerializationHandler<TAtom> atomSerializationHandler);

        IGremlinQuerySerializerBuilder ConfigureAssemblerFactory(Func<ISerializedGremlinQueryAssemblerFactory, ISerializedGremlinQueryAssemblerFactory> transformation);

        IGremlinQuerySerializer Build();
    }
}
