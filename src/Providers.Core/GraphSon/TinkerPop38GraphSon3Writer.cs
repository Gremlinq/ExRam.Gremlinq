using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.Core
{
    // Writes a byte and an sbyte, a strategy and a strategy type as Gremlin.Net 3.8 does (an sbyte as
    // gx:Byte and a byte as a bare number, a strategy as {"fqcn":…,"conf":{…}} and a strategy type as
    // g:Class), and a DateTimeOffset as gx:OffsetDateTime, for a server from TinkerPop 3.8 on.
    internal sealed class TinkerPop38GraphSon3Writer : GraphSON3Writer
    {
        public TinkerPop38GraphSon3Writer()
        {
            Serializers[typeof(DateTimeOffset)] = TinkerPop38Serializers.OffsetDateTime;
        }
    }
}
