using Gremlin.Net.Process.Traversal.Strategy;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class TinkerPop37Serializers
    {
        public static readonly IGraphSONSerializer Date = new DateSerializer();
        public static readonly IGraphSONSerializer Byte = new ByteSerializer();
        public static readonly IGraphSONSerializer Class = new ClassSerializer();
        public static readonly IGraphSONSerializer TraversalStrategy = new TraversalStrategySerializer();

        private sealed class DateSerializer : IGraphSONSerializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer) => GraphSONUtil.ToTypedValue("Date", ((DateTimeOffset)objectData).ToUnixTimeMilliseconds());
        }

        private sealed class ByteSerializer : IGraphSONSerializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer) => GraphSONUtil.ToTypedValue("Byte", (byte)objectData, "gx");
        }

        private sealed class ClassSerializer : IGraphSONSerializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                var type = (Type)objectData;

                return writer.ToDict(Activator.CreateInstance(type) ?? throw new IOException($"Cannot write {type}. maybe it is not a supported type"));
            }
        }

        private sealed class TraversalStrategySerializer : IGraphSONSerializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                var strategy = (AbstractTraversalStrategy)objectData;

                return GraphSONUtil.ToTypedValue(strategy.StrategyName, writer.ToDict(strategy.Configuration));
            }
        }
    }
}
