#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryDebugger
    {
        string? TryToString(ISerializedGremlinQuery serializedQuery);
    }
}
