using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IEdgePropertyGremlinQuery<TValue> : IGremlinQuery<Property<TValue>>
    {
    }
}
