using NullGuard;

namespace ExRam.Gremlinq.GraphElements
{
    public sealed class Property
    {
        [AllowNull] public string Key { get; set; }
        [AllowNull] public object Value { get; set; }
    }
}
