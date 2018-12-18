using NullGuard;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public class Property
    {
        [AllowNull] public string Key { get; set; }
        [AllowNull] public object Value { get; set; }
    }
}
