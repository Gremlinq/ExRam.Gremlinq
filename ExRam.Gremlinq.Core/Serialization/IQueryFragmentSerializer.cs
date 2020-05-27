using System;

namespace ExRam.Gremlinq.Core
{
    public interface IQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment);

        IQueryFragmentSerializer Override<TFragment>(Func<TFragment, IGremlinQueryEnvironment, Func<TFragment, object>, IQueryFragmentSerializer, object> serializer);
    }
}
