using NullGuard;

namespace ExRam.Gremlinq.GraphElements
{
    public abstract class Element
    {
        [AllowNull] public object Id { get; set; }
        [AllowNull] public string Label { get; set; }
    }
}
