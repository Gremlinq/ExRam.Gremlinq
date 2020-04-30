using System;

namespace ExRam.Gremlinq.Core
{
    public interface IQueryFragmentSerializer
    {
        object Serialize<TFragment>(TFragment fragment);

        IQueryFragmentSerializer Override<TFragment>(Func<TFragment, Func<TFragment, object>, IQueryFragmentSerializer, object> serializer);
    }
}