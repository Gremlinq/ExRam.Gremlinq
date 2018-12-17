using NullGuard;

namespace ExRam.Gremlinq.GraphElements
{
    internal sealed class EdgeImpl : IEdge
    {
        [AllowNull] public object Id { get; set; }
        [AllowNull] public string Label { get; set; }
    }
}
