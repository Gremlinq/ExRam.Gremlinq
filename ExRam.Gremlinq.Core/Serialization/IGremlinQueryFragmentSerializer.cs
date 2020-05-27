using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        IGremlinQueryFragmentSerializer Override<TFragment>(Func<TFragment, IGremlinQueryEnvironment, Func<TFragment, object>, IGremlinQueryFragmentSerializer, object> serializer);
    }
}
