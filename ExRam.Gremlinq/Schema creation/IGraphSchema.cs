using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphSchema
    {
        IGraphModel Model { get; }
        ImmutableList<PropertySchemaInfo> PropertySchemaInfos { get; }
        ImmutableList<(string, string, string)> Connections { get; }
    }
}