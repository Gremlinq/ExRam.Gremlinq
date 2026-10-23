using Gremlin.Net.Process.Traversal.Strategy;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.Core
{
    // Writes a byte and an sbyte as Gremlin.Net 3.8 does, an sbyte as gx:Byte and a byte as a bare
    // number, for a server from TinkerPop 3.8 on. Dates, types and strategies keep TinkerPop 3.7's forms.
    internal sealed class TinkerPop38GraphSon3Writer : GraphSON3Writer
    {
        public TinkerPop38GraphSon3Writer()
        {
            Serializers[typeof(DateTimeOffset)] = TinkerPop37Serializers.Date;
            Serializers[typeof(Type)] = TinkerPop37Serializers.Class;
            Serializers[typeof(AbstractTraversalStrategy)] = TinkerPop37Serializers.TraversalStrategy;
        }
    }
}
