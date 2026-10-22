using Gremlin.Net.Process.Traversal.Strategy;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class TinkerPop37GraphSon3Writer : GraphSON3Writer
    {
        public TinkerPop37GraphSon3Writer()
        {
            Serializers.Remove(typeof(sbyte));

            Serializers[typeof(DateTimeOffset)] = TinkerPop37Serializers.Date;
            Serializers[typeof(byte)] = TinkerPop37Serializers.Byte;
            Serializers[typeof(Type)] = TinkerPop37Serializers.Class;
            Serializers[typeof(AbstractTraversalStrategy)] = TinkerPop37Serializers.TraversalStrategy;
        }
    }
}
